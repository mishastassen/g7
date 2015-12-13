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
				  cookie: {maxAge: 3600000}}));

/*Session variable*/
var sess;

/*store whose friendlists and requests have been updated*/
var userFriendsRequestsUpdated;
var userFriendsUpdated;

/*Express urls*/
app.get('/',function(req,res){
	console.log('Empty url received');
	sess = req.session;
	if(sess.loggedin === true){
		res.redirect('/response');
	}
	else{
		res.send('Login first');
	}
});

app.post('/createAccount',function(req,res){
	console.log('User trying to create account');
	sess = req.session;
	if(sess.loggedin === true){
		res.send("Log out first");
	}
	else{
		var username = req.body.username,
			pass = req.body.password;
		if(username.length < 4 || pass.length < 6){
			console.log("Account denied");
			res.send("Username must have atleast 4 characters en password atleast 6");
		}
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
});

app.post('/login',function(req,res){
	console.log('User loggin in');
	sess = req.session;
	var username = req.body.username,
		pass = req.body.password;
	connection.query("SELECT * FROM Users WHERE AccountName ='" + username +"'", function(err, rows, fields) {
		if(err){
			console.log(err);
		}
		else if(rows.length === 0){
			console.log("Username bestaat niet");
			res.send("Wrong username");
		}
		else if(rows[0].Password === pass){
			sess.username = username;
			sess.UserId = rows[0].UserId;
			sess.loggedin = true;
			console.log('Password correct');
			res.send("you logged in!");
		}
		else{
			res.send("Wrong password");
		}
	});
});


app.get('/response',function(req,res){
	console.log('User asking for response');
	sess = req.session;
	if(sess.loggedin === true){
		res.send('Logged in as ' + sess.username);
	}else{
		res.send('Not logged in');
	}
});


app.get('/logout',function(req,res){
	req.session.destroy(function(err){
		if(err){
			console.log(err);
		}
		else{
			console.log('User logged out');
			res.send("You logged out");
		}
	});
});

app.get('/updateFriends',function(req,res){

});

app.get('/getFriendList',function(req,res){
	console.log('User requesting friend list');
	sess = req.session;
	if(sess.loggedin !== true){
		res.send('Log in first');
	}
	else{
		var UserId = sess.userId;
		connection.query("SELECT UserId, AccountName, Level_Progress FROM Users, Friends WHERE ((Friends.UserId_Sender ='" + UserId +"' AND Users.UserId = Users.UserId_Receiver) OR (Friends.UserId_Receiver ='" + UserId +"' AND Users.UserId = Users.UserId_Sender) )AND Status = 1", function(err, rows, fields) {
			if(err){
				console.log(err);
			}
			else{
				sess.FriendListUpdated = Date.now();
				res.json(rows);
			}
		});
	}
});

app.get('/getFriendRequests',function(req,res){
	console.log('User getting friend requests');
	sess = req.session;
	if(sess.loggedin !== true){
		res.send('Log in first');
	}
	else{
		var UserId = sess.userId;
		connection.query("SELECT UserId, AccountName, Status, UserId_Sender FROM Users, Friends WHERE ((Friends.UserId_Sender ='" + UserId +"' AND Users.UserId = Users.UserId_Receiver) OR (Friends.UserId_Receiver ='" + UserId +"' AND Users.UserId = Users.UserId_Sender) )AND Status = 0", function(err, rows, fields) {
			if(err){
				console.log(err);
			}
			else{
				sess.FriendRequestsUpdated = Date.now();
				res.json(rows);
			}
		});
	}
});

app.post('/sendFriendRequest',function(req,res){
	console.log('User sending friend request');
	sess = req.session;
	if(sess.loggedin !== true){
		res.send('Log in first');
	}
	else{
		var UserId = sess.userId;
		var FriendId = req.body.FriendId;
		connection.query("SELECT 1 FROM Friends WHERE (UserId_Sender = '"+ UserId +"' AND UserId_Receiver '" + FriendId + "') OR (UserId_Sender = '"+ FriendId +"' AND UserId_Receiver '" + UserId + "') AND ORDER BY UserId_Sender LIMIT 1", function (err, rows, fields) {
			if)(err){
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
						userFriendsRequestsUpdated.FriendId = true;
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
	if(sess.loggedin !== true){
		res.send('Log in first');
	}
	else{
		var UserId = sess.userId;
		var FriendId = req.body.FriendId;
		connection.query("UPDATE Friends Set Status = 1 WHERE UserId_Sender = '" + FriendId + "' AND UserId_Receiver = '" + UserId + "'", function(err, rows, fields) {
			if(err){
				console.log(err);
			}
			else{
				userFriendsRequestsUpdated.FriendId = true;
				userFriendsUpdated.FriendId = true;
				res.send('Friend request accepted');
			}
		});
	}
});

app.post('/getHigscores',function(req,res){
	console.log('User requesting highscores');
	sess = req.session;
	if(sess.loggedin !== true){
		res.send('Log in first');
	}
	else{
		
	}
});

app.post('/updateHighscores',function(req,res){
	console.log('User updating highscores');
	sess = req.session;
	if(sess.loggedin !== true){
		res.send('Log in first');
	}
	else{
		
	}
});

app.post('/updateAvatar',function(req,res){
	console.log('User updating avatar');
	sess = req.session;
	if(sess.loggedin !== true){
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
