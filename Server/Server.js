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
				  cookie: {}}));

/*Session variable*/
var sess;

/*Express urls*/
app.get('/',function(req,res){
	console.log('Empty url received');
	sess = req.session;
	if(sess.loggedin === true){
		res.redirect('/response');
	}
	else{
		res.end('Login first');
	}
});

app.post('/createAccount',function(req,res){
	console.log('User trying to create account');
	sess = req.session;
	if(sess.loggedin === true){
		res.end("Log out first");
	}
	else{
		var username = req.body.username,
			pass = req.body.password;
		if(username.length < 4 || pass.length < 6){
			console.log("Account denied");
			res.end("Username must have atleast 4 characters en password atleast 6");
		}
		connection.query("SELECT 1 FROM Users WHERE AccountName = '"+username+"' ORDER BY AccountName LIMIT 1", function (err, rows, fields) {
			if(err){
				console.log(err);
			}
			else if(rows.length  > 0){
				console.log("Account denied");
				res.end("Username already in use");
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
			res.end("Wrong username");
		}
		else if(rows[0].Password === pass){
			sess.username = username;
			sess.UserId = rows[0].UserId;
			sess.loggedin = true;
			console.log('Password correct');
			res.end("you logged in!");
		}
		else{
			res.end("Wrong password");
		}
	});
});


app.get('/response',function(req,res){
	console.log('User asking for response');
	sess = req.session;
	if(sess.loggedin === true){
		res.end('Logged in as ' + sess.username);
	}else{
		res.end('Not logged in');
	}
});


app.get('/logout',function(req,res){
	req.session.destroy(function(err){
		if(err){
			console.log(err);
		}
		else{
			console.log('User logged out');
			res.end("You logged out");
		}
	});
});

/*Start server*/
var server = app.listen(port, function () {
  var port = server.address().port;

  console.log('Server listening at %s', port);
});
