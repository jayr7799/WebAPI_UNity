const mongoose = require("mongoose");
const Player = require("./models/player");
const {nanoid} = require("nanoid");

mongoose.connect("");

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

