/* global __dirname */
var express = require('express');
var session = require('express-session');
var bodyParser = require('body-parser');

var port = 8008;
var app = express();

app.set('responses', __dirname + '/responses');

app.use( session({secret: 'Geheim'} );
app.use( express.json() );
app.use( bodyParser.json() );  

var sess;

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

app.post('/login',function(req,res){
	console.log('User loggin in');
	sess = req.session;
	var username = req.body.username,
		pass = req.body.color;
	if (username === 'admin' && pass === 'root'){
		sess.username = admin;
		sess.loggedin = true;
		console.log('Password correct');
	}
});

app.get('/response',function(req,res){
	console.log('User asking for response');
	sess = req.session;
	if(sess.loggedin === true){
		res.end('Logged in as' + sess.username);
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
		}
	});
});

app.listen(port,function(){
console.log("App Started listening on port" + port);
});