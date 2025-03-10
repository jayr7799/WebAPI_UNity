const mongoose = require("mongoose");

const playerSchema = new mongoose.Schema({
    playerid:{type:String, unique:true},
    name:String,
    firstName:String,
    lastName:String,
    joined:{type:Date, default:Date.now},
    timesPlayed:Number,
    wins:Number,
    score:Number
})
const player = mongoose.model("Player", playerSchema);

module.exports = player;