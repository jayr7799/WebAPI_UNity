const mongoose = require("mongoose");
const Player = require("./models/player");
const {nanoid} = require("nanoid");

mongoose.connect("mongodb+srv://jayrossi204:Trace7799@cluster0.jtkdp.mongodb.net/GamesDB?retryWrites=true&w=majority&appName=Cluster0");

async function addPlayer(){
    await Player.create({
        playerid:nanoid(8),
        name:"Megan",
        level:8,
        score:1000
    });
    console.log("Player Added");
    mongoose.connection.close();
}

addPlayer();

