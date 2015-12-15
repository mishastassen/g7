/* global __dirname */
var express = require('express');
var session = require('express-session');
var bodyParser = require('body-parser');
var cookieParser = require('cookie-parser');
var mysql = require('mysql');

/*Connect to MySQL database */
var connection = mysql.createConnection({
  host     : 'localhost',
  user     : 'ewi3620tu7',
  password : 'ayRoHef3',
  database : 'ewi3620tu7'
});
connection.connect();

/*select port for server*/
var port = 8088;

/*Initializng express */
var app = express();
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
  extended: true
}));
app.use(cookieParser());
app.use( session({cookieName: 'session', 
				  secret: 'Geheim',
				  resave: true,
				  saveUninitialized: true,
				  cookie: {maxAge: 10000}}));
app.use(touchUser());

/*Session variable*/
var sess;

/*store whose friendlists and requests have been updated*/
var userFriendRequestsUpdated = {};
var userFriendsUpdated = {};

/*store who is online*/
var onlineUsers = {};

/*Express urls*/
app.get('/',function(req,res){
	console.log('Empty url received');
	sess = req.session;
	if(sess.User !== undefined){
		res.redirect('/response');
	}
	else{
		res.send('Login first');
	}
});

app.post('/createAccount',function(req,res){
	console.log('User trying to create account');
	sess = req.session;
	if(sess.User !== undefined){
		res.send("Log out first");
	}
	else{
		var username = req.body.username,
			pass = req.body.password;
		if(username.length < 4 || pass.length < 6){
			console.log("Account denied");
			res.send("Username must have atleast 4 characters en password atleast 6");
		}
		else{
			connection.query("SELECT 1 FROM Users WHERE AccountName = '"+username+"' ORDER BY AccountName LIMIT 1", function (err, rows, fields) {
				if(err){
					console.log(err);
				}
				else if(rows.length  > 0){
					console.log("Account denied");
					res.send("Username already in use");
				}
				else{
					console.log("Creating new account " + username);
					connection.query("INSERT INTO Users(AccountName,Password) VALUES ('" + username + "','" + pass + "')",function(err, rows, fields) {
						if(err){
							console.log(err);
						}
						else{
							console.log("Account succesfully created");
							res.end("Account created");
						}
					});
				}
			});
		}
	}
});

app.post('/login',function(req,res){
	console.log('User loggin in');
	sess = req.session;
	var username = req.body.username,
		pass = req.body.password;
	connection.query("SELECT UserId, AccountName,Password,LevelProgress FROM Users, Levels WHERE AccountName ='" + username +"' AND Users.LevelProgressId = Levels.LevelId", function(err, rows, fields) {
		if(err){
			console.log(err);
		}
		else if(rows.length === 0){
			console.log("Username bestaat niet");
			res.send("Wrong username");
		}
		else if(rows[0].Password === pass){
			var user = new User(rows[0].UserId,username,true,Date.now(),rows[0].LevelProgress);
			onlineUsers[user.UserId] = user;
			sess.User = onlineUsers[user.UserId];
			console.log('Password correct');
			res.json(user);
		}
		else{
			res.send("Wrong password");
		}
	});
});


app.get('/response',function(req,res){
	console.log('User asking for response');
	sess = req.session;
	if(sess.User !== undefined){
		res.send('Logged in as ' + sess.User.Username);
	}else{
		res.send('Not logged in');
	}
});


app.get('/logout',function(req,res){
	if(sess.User !== undefined){
		if(onlineUsers[sess.User.UserId] !== undefined){
			onlineUsers[sess.User.UserId].LoggedIn = false;
		}
		delete req.session.User;
		req.session.destroy(function(err){
			if(err){
				console.log(err);
			}
			else{
				console.log('User logged out');
				res.send("You logged out");
			}
		});
	}
	res.send("Already logged out");
});

app.get('/updateUsers',function(req,res){
	console.log('User requesting online users');
	sess = req.session;
	if(sess.User === undefined){
		res.send('Log in first');
	}
	else{
		res.json(onlineUsers);
		onlineUsers[sess.User.UserId].lastUpdate = Date.now();
	}
});

app.get('/updateFriends',function(req,res){
	console.log('User requesting friend list');
	sess = req.session;
	if(sess.User === undefined){
		res.send('Log in first');
	}
	else{
		var UserId = sess.User.UserId;
		if(sess.lastFriendListUpdate === undefined){
			res.redirect('/getFriendlist');
		}
		else if(userFriendsUpdated.UserId === true){
			res.redirect('/getFriendlist');
			userFriendsUpdated.UserId = false;
		}
		else if(Date.now() - sess.lastFriendListUpdate > 300000){
			res.redirect('/getFriendlist');
		}
		else {
			res.send('no need to update friendlist');
		}
	}
});	

app.get('/updateFriendRequests',function(req,res){
	console.log('User requesting friend requests');
	sess = req.session;
	if(sess.User === undefined){
		res.send('Log in first');
	}
	else{
		var UserId = sess.User.UserId;		
		if(sess.lastFriendRequestUpdate === undefined){
			res.redirect('/getFriendRequests');
		}
		else if(userFriendRequestsUpdated.UserId === true){
			res.redirect('/getFriendRequests');
			userFriendRequestsUpdated.UserId = false;
		}
		else if(Date.now() - sess.lastFriendRequestUpdate > 300000){
			res.redirect('/getFriendRequests');
		}
		else {
			res.send('no need to update friendrequests');
		}
	}
});

app.get('/getFriendList',function(req,res){
	console.log('Updating user friend list');
	sess = req.session;
	if(sess.User === undefined){
		res.send('Log in first');
	}
	else{
		var UserId = sess.User.UserId;
		connection.query("SELECT UserId, AccountName, LevelProgress FROM Users, Friends, Levels WHERE Users.LevelProgressId = Levels.LevelId AND ((Friends.UserId_Sender ='" + UserId +"' AND Users.UserId = Friends.UserId_Receiver) OR (Friends.UserId_Receiver ='" + UserId +"' AND Users.UserId = Friends.UserId_Sender) )AND Status = 1", function(err, rows, fields) {
			if(err){
				console.log(err);
			}
			else{
				sess.lastFriendListUpdate = Date.now();
				var friends = new Array();
				rows.forEach(function(index){
					var friend = new User(index.UserId,index.AccountName,false,0,index.LevelProgress);
					friends.push(friend);
				});
				res.json(friends);
			}
		});
	}
});

app.get('/getFriendRequests',function(req,res){
	console.log('Updating user friend requests');
	sess = req.session;
	if(sess.User === undefined){
		res.send('Log in first');
	}
	else{
		var UserId = sess.User.UserId;
		connection.query("SELECT UserId, AccountName, Levels.LevelProgress FROM Users, Friends, Levels WHERE Users.LevelProgressId = Levels.LevelId AND ((Friends.UserId_Sender ='" + UserId +"' AND Users.UserId = Friends.UserId_Receiver) OR (Friends.UserId_Receiver ='" + UserId +"' AND Users.UserId = Friends.UserId_Sender) )AND Status = 0", function(err, rows, fields) {
			if(err){
				console.log(err);
			}
			else{
				sess.lastFriendRequestUpdate = Date.now();
				var friendRequests = new Array();
				rows.forEach(function(index){
					var requested = new User(index.UserId,index.AccountName,false,0,index.LevelProgress);
					friendRequests.push(requested);
				});
				res.json(friendRequests);
			}
		});
	}
});

app.post('/sendFriendRequest',function(req,res){
	console.log('User sending friend request');
	sess = req.session;
	if(sess.User === undefined){
		res.send('Log in first');
	}
	else{
		var UserId = sess.User.UserId;
		var FriendId = req.body.FriendId;
		connection.query("SELECT 1 FROM Friends WHERE (UserId_Sender = '"+ UserId +"' AND UserId_Receiver '" + FriendId + "') OR (UserId_Sender = '"+ FriendId +"' AND UserId_Receiver '" + UserId + "') AND ORDER BY UserId_Sender LIMIT 1", function (err, rows, fields) {
			if(err){
				console.log(err);
			}
			else if (rows.length  > 0){
				res.send('Request bestaat al');
			}
			else{
				connection.query("INSERT INTO Friends VALUES ('" + UserId + "','" + FriendId + "',0)", function(err, rows, fields) {
					if(err){
						console.log(err);
					}
					else{
						userFriendRequestsUpdated.FriendId = true;
						res.send('Friend request sent');
					}
				});
			}
		});
	}
});

app.post('/acceptFriendRequest',function(req,res){
	console.log('User accepting friend request');
	sess = req.session;
	if(sess.User === undefined){
		res.send('Log in first');
	}
	else{
		var UserId = sess.User.UserId;
		var FriendId = req.body.FriendId;
		connection.query("UPDATE Friends Set Status = 1 WHERE UserId_Sender = '" + FriendId + "' AND UserId_Receiver = '" + UserId + "'", function(err, rows, fields) {
			if(err){
				console.log(err);
			}
			else{
				userFriendRequestsUpdated.FriendId = true;
				userFriendsUpdated.FriendId = true;
				res.send('Friend request accepted');
			}
		});
	}
});

app.get('/getOnlinePlayers',function(req,res){
	console.log('User requesting online players');
	sess = req.session;
	if(sess.User === undefined){
		res.send('Log in first');
	}
	else{
		
	}
});

app.post('/getHigscores',function(req,res){
	console.log('User requesting highscores');
	sess = req.session;
	if(sess.User === undefined){
		res.send('Log in first');
	}
	else{
		
	}
});

app.post('/updateHighscores',function(req,res){
	console.log('User updating highscores');
	sess = req.session;
	if(sess.User === undefined){
		res.send('Log in first');
	}
	else{
		
	}
});

app.post('/updateAvatar',function(req,res){
	console.log('User updating avatar');
	sess = req.session;
	if(sess.User === undefined){
		res.send('Log in first');
	}
	else{
		
	}
});

/*Start server*/
var server = app.listen(port, function () {
	var port = server.address().port;
	console.log('Server listening at %s', port);
});
wait10sec();	//Start cleaning offline users
wait10min();	//Start cleaning open requests



/*Function to run every 10 seconds*/
function cleanOfflineUsers(callback){	//Removing users that went offline
	console.log('cleaning offline users');
	for (var UserId in onlineUsers){
		if (onlineUsers.hasOwnProperty(UserId)){
			if(onlineUsers[UserId].LoggedIn === false || (Date.now()- onlineUsers[UserId].lastUpdate > 10000)){
				delete onlineUsers[UserId];
			}
		}
	}
	callback();
}

function wait10sec(){
    setTimeout(function(){
        cleanOfflineUsers(wait10sec);
    }, 10000);
}

/*Function to run every 10 minutes*/
function cleanOpenRequests(callback){	//Clean open requests and friendlist update arrays
	console.log('cleaning open requests');
	userFriendRequestsUpdated = {};
	userFriendsUpdated = {};
	callback();
}

function wait10min(){
    setTimeout(function(){
        cleanOpenRequests(wait10min);
    }, 600000);
}



/*Objects*/

/*Player*/
function User(UserId,Username,LoggedIn,lastUpdate,levelProgress){
	this.UserId = UserId;
	this.Username = Username;
	this.LoggedIn = LoggedIn;
	this.lastUpdate = lastUpdate;
	this.levelProgress = levelProgress;
}

/*Custom MiddleWare*/
function touchUser() {
	return function(req,res,next){
		sess = req.session;
		if(sess.User != undefined){
			onlineUsers[sess.User.UserId].lastUpdate = Date.now();
		}
		next();
	}
}