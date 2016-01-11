/* global __dirname */
var express = require('express');
var session = require('express-session');
var bodyParser = require('body-parser');
var cookieParser = require('cookie-parser');
var mysql = require('mysql');
var getIP = require('ipware')().get_ip;


/*Setup MySQL database pool*/
var MySQLpool = mysql.createPool({
  host     : 'localhost',
  user     : 'ewi3620tu7',
  password : 'ayRoHef3',
  database : 'ewi3620tu7',
  multipleStatements: true
});

/*select port for server*/
var port = 8088;

/*Initializng express and setting up middleware */
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
app.use(setIp());
app.use(touchUser());

/*Session variable*/
var sess;

/*store whose friendlists and requests have been updated*/
var userFriendRequestsUpdated = {};
var userFriendsUpdated = {};

/*store who is online*/
var onlineUsers = {};

/*Store messages*/
var Messages = {};

/*Express urls*/
app.get('/',function(req,res){
	console.log('Empty url received');
	sess = req.session;
	if(sess.UserId !== undefined){
		res.redirect('/response');
	}
	else{
		res.send('Login first');
	}
});

app.post('/createAccount',function(req,res){
	console.log('User trying to create account');
	sess = req.session;
	if(sess.UserId !== undefined){
		res.send("Log out first");
	}
	else{
		var username = req.body.username,
			pass = req.body.password,
			color = req.body.color,
			sex = req.body.sex;
		if(username.length < 4 ){
			console.log("Username denied");
			res.send("Username must have at least 4 characters");
		} else if (pass.length < 6) {
			console.log("Password denied");
			res.send("Password must have at least 6 characters");
		}
		else{
			MySQLpool.getConnection(function(err, connection) {
				if(err){
					console.log(err);
				}
				else{
					connection.query("SELECT 1 FROM Users WHERE AccountName = '"+username+"' ORDER BY AccountName LIMIT 1", function (err, rows, fields) {
						if(err){
							console.log(err);
							connection.release();
						}
						else if(rows.length  > 0){
							console.log("Account denied");
							res.send("Username is already taken");
							connection.release();
						}
						else{
							console.log("Creating new account " + username);
							connection.query("INSERT INTO Users(AccountName,Password) VALUES ('" + username + "','" + pass + "')",function(err, rows, fields) {
								if(err){
									connection.release();
									console.log(err);
								}
								else{
									console.log("Account accepted");
									UserId = rows.insertId;
									connection.query("SELECT AvatarId FROM Avatars WHERE Colour = '" + color + "' AND Sex = '" + sex + "' LIMIT 1",function(err, rows, fields) {
										if(err){
											connection.release();
											console.log(err);
										}
										else if(rows.length > 0){
											AvatarId = rows[0].AvatarId;
											connection.query("INSERT INTO PlayerAvatar(UserId,AvatarId) VALUES ('" + UserId + "','" + AvatarId + "')",function(err, rows, fields) {
												connection.release();
												if(err){
													console.log(err);
												}
												else{
													console.log("Account succesfully created using existing avatar");
													res.end("Account created");
												}
											});
										}
										else{
											connection.query("BEGIN; INSERT INTO Avatars(Sex,Colour) VALUES ('" + sex + "','" + color + "'); INSERT INTO PlayerAvatar(UserId,AvatarId) VALUES ('" + UserId + "',LAST_INSERT_ID()); COMMIT",function(err, rows, fields) {
												connection.release();
												if(err){
													console.log(err);
												}
												else{
													console.log("Account succesfully created using new avatar");
													res.end("Account created");
												}
											});
										}
									});
								}
							});
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
	MySQLpool.getConnection(function(err, connection) {
		if(err){
			console.log(err);
		}
		else{
			connection.query("SELECT UserId, AccountName,Password,LevelProgress FROM Users, Levels WHERE AccountName ='" + username +"' AND Users.LevelProgressId = Levels.LevelId", function(err, rows, fields) {
				if(err){
					connection.release();
					console.log(err);
				}
				else if(rows.length === 0){
					connection.release();
					console.log("Username doesn't exist");
					res.send("Wrong username");
				}
				else if(rows[0].Password === pass){
					var UserId = rows[0].UserId;
					var LevelProgress = rows[0].LevelProgress;
					console.log('Password correct');
					connection.query("SELECT Colour, Sex FROM Avatars, PlayerAvatar WHERE Avatars.AvatarId = PlayerAvatar.AvatarId AND PlayerAvatar.UserId = '" + UserId + "'",function(err, rows, fields){
						connection.release();
						if(err){
							console.log(err);
						}
						else{
							var playerColor = rows[0].Colour;
							var Sex = rows[0].Sex;
							var user = new User(UserId,username,true,Date.now(),LevelProgress,sess.ipInfo,playerColor,Sex);
							onlineUsers[user.UserId] = user;
							console.log(user.UserId);
							sess.UserId = user.UserId;
							res.json(user);
							console.log("User logged in");
						}
					});
				}
				else{
					connection.release();
					res.send("Wrong password");
				}
			});
		}
	});
});


app.get('/response',function(req,res){
	console.log('User asking for response');
	sess = req.session;
	if(sess.UserId !== undefined){
		res.send('Logged in as ' + onlineUsers[sess.UserId].Username);
	}else{
		res.send('Not logged in');
	}
});


app.get('/logout',function(req,res){
	sess = req.session;
	if(sess.UserId !== undefined){
		if(onlineUsers[sess.UserId] !== undefined){
			onlineUsers[sess.UserId].LoggedIn = false;
		}
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
	else{
		res.send("Already logged out");
	}
});

app.get('/updateUsers',function(req,res){
	console.log('User requesting online users');
	sess = req.session;
	if(sess.UserId === undefined){
		res.send('Log in first');
	}
	else{
		res.json(onlineUsers);
	}
});

app.get('/updateFriends',function(req,res){
	console.log('User requesting friend list');
	sess = req.session;
	if(sess.UserId === undefined){
		res.send('Log in first');
	}
	else{
		var UserId = sess.UserId;
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
	if(sess.UserId === undefined){
		res.send('Log in first');
	}
	else{
		var UserId = sess.UserId;		
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
	if(sess.UserId === undefined){
		res.send('Log in first');
	}
	else{
		var UserId = sess.UserId;
		MySQLpool.getConnection(function(err, connection) {
			if(err){
				console.log(err);
			}
			else {
				connection.query("SELECT UserId, AccountName, LevelProgress FROM Users, Friends, Levels WHERE Users.LevelProgressId = Levels.LevelId AND ((Friends.UserId_Sender ='" + UserId +"' AND Users.UserId = Friends.UserId_Receiver) OR (Friends.UserId_Receiver ='" + UserId +"' AND Users.UserId = Friends.UserId_Sender) )AND Status = 1", function(err, rows, fields) {
					connection.release();
					if(err){
						console.log(err);
					}
					else{
						sess.lastFriendListUpdate = Date.now();
						var friends = new Array();
						rows.forEach(function(index){
							var friend = new User(index.UserId,index.AccountName,false,0,index.LevelProgress,0);
							friends.push(friend);
						});
						res.json(friends);
					}
				});
			}
		});
	}
});

app.get('/getFriendRequests',function(req,res){
	console.log('Updating user friend requests');
	sess = req.session;
	if(sess.UserId === undefined){
		res.send('Log in first');
	}
	else{
		var UserId = sess.UserId;
		MySQLpool.getConnection(function(err, connection) {
			if(err){
				console.log(err);
			}
			else {
				connection.query("SELECT UserId, AccountName, Levels.LevelProgress FROM Users, Friends, Levels WHERE Users.LevelProgressId = Levels.LevelId AND ((Friends.UserId_Sender ='" + UserId +"' AND Users.UserId = Friends.UserId_Receiver) OR (Friends.UserId_Receiver ='" + UserId +"' AND Users.UserId = Friends.UserId_Sender) )AND Status = 0", function(err, rows, fields) {
					connection.release();
					if(err){
						console.log(err);
					}
					else{
						sess.lastFriendRequestUpdate = Date.now();
						var friendRequests = new Array();
						rows.forEach(function(index){
							var requested = new User(index.UserId,index.AccountName,false,0,index.LevelProgress,0);
							friendRequests.push(requested);
						});
						res.json(friendRequests);
					}
				});
			}
		});
	}
});

app.post('/sendFriendRequest',function(req,res){
	console.log('User sending friend request');
	sess = req.session;
	if(sess.UserId === undefined){
		res.send('Log in first');
	}
	else{
		var UserId = sess.UserId;
		var FriendId = req.body.FriendId;
		MySQLpool.getConnection(function(err, connection) {
			if(err){
				console.log(err);
			}
			else {
				connection.query("SELECT 1 FROM Friends WHERE (UserId_Sender = '"+ UserId +"' AND UserId_Receiver '" + FriendId + "') OR (UserId_Sender = '"+ FriendId +"' AND UserId_Receiver '" + UserId + "') AND ORDER BY UserId_Sender LIMIT 1", function (err, rows, fields) {
					if(err){
						console.log(err);
						connection.release();
					}
					else if (rows.length  > 0){
						res.send('Request bestaat al');
						connection.release();
					}
					else{
						connection.query("INSERT INTO Friends VALUES ('" + UserId + "','" + FriendId + "',0)", function(err, rows, fields) {
							connection.release();
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
	}
});

app.post('/acceptFriendRequest',function(req,res){
	console.log('User accepting friend request');
	sess = req.session;
	if(sess.UserId === undefined){
		res.send('Log in first');
	}
	else{
		var UserId = sess.UserId;
		var FriendId = req.body.FriendId;
		MySQLpool.getConnection(function(err, connection) {
			if(err){
				console.log(err);
			}
			else {
				connection.query("UPDATE Friends Set Status = 1 WHERE UserId_Sender = '" + FriendId + "' AND UserId_Receiver = '" + UserId + "'", function(err, rows, fields) {
					if(err){
						connection.release();
						console.log(err);
					}
					else{
						connection.release();
						userFriendRequestsUpdated.FriendId = true;
						userFriendsUpdated.FriendId = true;
						res.send('Friend request accepted');
					}
				});
			}
		});
	}
});

app.post('/sendMessage',function(req,res){
	console.log('User sending message');
	sess = req.session;
	if(sess.UserId === undefined){
		res.send('Log in first');
	}
	else{
		var UserId = sess.UserId;
		var ReceipId = req.body.ReceipId;
		if(onlineUsers[ReceipId] !== undefined){
			if(onlineUsers[ReceipId].LoggedIn === true){
				Messages[ReceipId] = req.body;
				Messages[ReceipId].Timestamp = Date.now();
			}
			else{
				res.send("Can only send messages to online players");
			}
		}
		else{
			res.send("Can only send messages to online players");
		}
	}
});

app.get('/getMessages',function(req,res){
	console.log('User requesting messages');
	sess = req.session;
	if(sess.UserId === undefined){
		res.send('Log in first');
	}
	else{
		var UserId = sess.UserId;
		if(Messages[UserId] === undefined){
			res.send('No messages');
		}
		else{
			res.json(Messages[UserId]);
			delete Messages[UserId];
		}
	}
});

app.post('/getHigscores',function(req,res){
	console.log('User requesting highscores');
	sess = req.session;
	if(sess.UserId === undefined){
		res.send('Log in first');
	}
	else{
		var UserId = sess.UserId;
		var LevelId = req.body.LevelId;
		var response = {};
		MySQLpool.getConnection(function(err, connection) {
			if(err){
				console.log(err);
			}
			else {
				connection.query("SELECT HS.Highscore, P1.AccountName, P2.AccountName FROM HighScores AS HS JOIN Users AS P1 ON HS.UserId_Player1=P1.UserId JOIN Users As P2 ON HS.UserId_Player2=P2.UserId WHERE HS.LevelId = '" + LevelId +"' ORDER BY HS.Highscore LIMIT 10",function(err,rows,fields){
					if(err){
						connection.release();
						console.log(err);
					}
					else{
						response.top10 = rows;
						connection.query("SELECT HS.Highscore, P1.AccountName, P2.AccountName FROM HighScores AS HS JOIN Users AS P1 ON HS.UserId_Player1=P1.UserId JOIN Users As P2 ON HS.UserId_Player2=P2.UserId WHERE HS.UserId_Player1 = '" + UserId +"' OR HS.UserId_Player2 = '" + UserId +"' ORDER BY HS.Highscore LIMIT 1",function(err,rows,fields){
							connection.release();
							if(err){
								console.log(err);
							}
							else{
								response.bestTime = rows;
								res.json(response);
							}
						});
					}
				});
			}
		});
	}
});

app.post('/updateHighscores',function(req,res){
	console.log('User updating highscores');
	sess = req.session;
	if(sess.UserId === undefined){
		res.send('Log in first');
	}
	else{
		var Player1 = sess.UserId;
		var LevelId = req.body.LevelId;
		var Player2 = req.body.Player2Id;
		var Highscore = req.body.Highscore;
		MySQLpool.getConnection(function(err, connection) {
			if(err){
				console.log(err);
			}
			else{
				connection.query("INSERT INTO HighScores (UserId_Player1,UserId_Player2,LevelId,HighScore) VALUES (" + Player1 + "," + Player2 + "," + LevelId + "," + Highscore + ")",function(err, rows, fields){
					connection.release();
					if(err){
						console.log(err);
					}
					else{
						res.send("Succes");
					}
				});
			}
		});
	}
});


app.post('/updateAvatar',function(req,res){
	console.log('User updating avatar');
	sess = req.session;
	if(sess.UserId === undefined){
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
wait60sec();	//Start cleaning offline users
wait10min();	//Start cleaning old vars

/*Process functions*/
process.on("exit", function() {
	pool.end(function (err) {
		if(err){
			console.log(err);
		}
	});
});

/*Function to run every 60 seconds*/
function cleanOfflineUsers(callback){	//Removing users that went offline
	console.log('cleaning offline users');
	for (var UserId in onlineUsers){
		if (onlineUsers.hasOwnProperty(UserId)){
			if(onlineUsers[UserId].LoggedIn === false || (Date.now()- onlineUsers[UserId].lastUpdate > 60000)){
				delete onlineUsers[UserId];
			}
		}
	}
	callback();
}

function wait60sec(){
    setTimeout(function(){
        cleanOfflineUsers(wait60sec);
    }, 60000);
}

/*Function to run every 10 minutes*/
function cleanVars(callback){
	console.log('cleaning variables');	//Clean open requests and friendlist update arrays
	userFriendRequestsUpdated = {};
	userFriendsUpdated = {};
	
	for (var ReceipId in Messages){		//Clean old messages
		if (Messages.hasOwnProperty(ReceipId)){
			if(Date.now()- Messages[ReceipId].timestamp > 30000){
				delete Messages[ReceipId];
			}
		}
	}
	callback();
}

function wait10min(){
    setTimeout(function(){
        cleanVars(wait10min);
    }, 600000);
}

/*Objects*/

/*Player*/
function User(UserId,Username,LoggedIn,lastUpdate,levelProgress,ipInfo,playerColor,Sex){
	this.UserId = UserId;
	this.Username = Username;
	this.LoggedIn = LoggedIn;
	this.lastUpdate = lastUpdate;
	this.levelProgress = levelProgress;
	this.ipInfo = ipInfo;
	this.playerColor = playerColor;
	this.Sex = Sex;
}

/*Custom middleware*/
function touchUser() {
	return function(req,res,next){
		sess = req.session;
		if(sess.UserId !== undefined){
			if(onlineUsers[sess.UserId] === undefined){
				req.session.destroy(function(err){
					if(err){
						console.log(err);
					}
					else{
						console.log('User timed out');
						res.send("You timed out");
					}
				});
			}else{
				onlineUsers[sess.UserId].lastUpdate = Date.now();
				next();
			}
		}
		else{
			next();
		}
	}
}

function setIp(){
	return function(req,res,next){
		var ipInfo = getIP(req);
		req.session.ipInfo = ipInfo;
		next();
	}
}
