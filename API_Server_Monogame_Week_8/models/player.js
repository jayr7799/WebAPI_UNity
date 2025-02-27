const mongoose = require("mongoose");

// const playerSchema = new mongoose.Schema({
//     playerid:{type:String, unique:true},
//     name:String,
//     level:Number,
//     score:Number
// })
const playerSchema = new mongoose.Schema({
    playerid:{type:String, unique:true},
    name:String,
    firstName:String,
    lastName:String,
    joined:{type:Date, default:Date.now},
    score:Number
})
const player = mongoose.model("Player", playerSchema);

module.exports = player;