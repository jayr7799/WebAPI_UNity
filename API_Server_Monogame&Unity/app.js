const express = require("express");
const mongoose = require("mongoose");
const fs = require("fs"); //file system
const cors = require("cors");
const bodyParser = require("body-parser");
const {nanoid} = require("nanoid");
const path = require("path");
const app = express();

app.use(express.static(path.join(__dirname, "public")));
app.use(express.json());
app.use(cors()); //allows us to make requests from game
app.use(bodyParser.json());
app.use(express.urlencoded({ extended: true }));

const Player = require("./models/player");

//connection for mongoDB
mongoose.connect("");//("mongodb://localhost:27017/gamedb");

const db = mongoose.connection;

db.on("error", console.error.bind(console, "MongoDB Connection Error"));
db.once("open", ()=>{console.log("Connected to MongoDB database")});

app.get("/users", (req,res)=>{
    res.sendFile(path.join(__dirname,"public","users.html"));
});
app.get("/leaderboard", (req,res)=>{
    res.sendFile(path.join(__dirname,"public","leaderboard.html"));
});
app.get("/update", (req,res)=>{
    res.sendFile(path.join(__dirname, "public","update.html"));
});
app.get("/playerLeaderBoard", async (req, res)=>{
    try
    {
        const players = await Player.find().sort({ wins: -1 }).limit(10);
        if(!players)
        {
            return res.status(404).json({error:"Players not found"});
        }
        res.json(players);
        console.log(players);
    }
    catch(error)
    {
        res.status(500).json({error:"Failed to get players"})
    }
});
app.get("/player", async (req, res)=>{
    try
    {
        const players = await Player.find();
        if(!players)
        {
            return res.status(404).json({error:"Players not found"});
        }
        res.json(players);
        console.log(players);
    }
    catch(error)
    {
        res.status(500).json({error:"Failed to get players"})
    }
});
app.get("/player/:playerid", async (req, res)=>{
    console.log("Here" + req.params.playerid);
    try
    {
        const player = await Player.findOne({playerid:req.params.playerid})
        if(!player)
        {
            return res.status(404).json({error:"Player not found"});
        }
        res.json(player);
    }
    catch(error)
    {
        res.status(500).json({error:"Failed to get player from ID"})
    }
});

app.post("/sentdataToDB", async (req,res)=>{
    try {
        const newPlayerData = req.body;
        console.log(JSON.stringify(newPlayerData,null,2));
        const newPlayer = new Player({
            playerid:nanoid(8), //tells id to be 8 long
            name:newPlayerData.name,
            firstName:newPlayerData.firstName,
            lastName:newPlayerData.lastName,
            dataJoined:newPlayerData.dataJoined,
            timesPlayed:newPlayerData.timesPlayed,
            wins:newPlayerData.wins,
            score:newPlayerData.score
        });
        //save to database
        await newPlayer.save();
        res.json({message:"Player added to DB", playerid:newPlayer.playerid, name:newPlayer.name});
    }
    catch(error) {
        res.status(500).json({error:"Failed to add player"});
    }
});

//update player
app.post("/updatePlayer", async (req, res) => {
    console.log("Received data:", req.body); 
    const playerData = req.body; // This will contain playerid, name, score, etc.
    const player = await Player.findOne({ playerid: playerData.playerid });

    if (!player) {
        return res.status(404).json({ message: "Player Not Found" });
    }

    // Update player data
    player.name = playerData.name;
    player.firstName = playerData.firstName || " ";
    player.lastName = playerData.lastName || " ";
    player.joined = Date.now();
    player.timesPlayed = playerData.timesPlayed;
    player.wins = playerData.wins;
    player.score = playerData.score;

    await player.save();

    res.json({ message: "Player updated", player });
});

app.delete("/deletePlayer", async (req,res)=>{
    try
    {
        const playerName = req.body;
        const player = await Player.findOne({playerid:playerName.playerid});
        if(player.length === 0) //exactly equal too, info and data type are equal
        {       
            return res.status(404).json({error:"{Failed to find item}"});
        }
        const deletedPerson = await Player.findOneAndDelete(playerName);
        res.json({message:"Item Deleted!"});
    }
    catch(err)
    {
        console.log(err);
        //res.status(404).json({error:"{Failed to find item}"});
    }
});
app.delete("/deletePlayerUnity", async (req, res) => {
    try {
        // Use query parameters instead of body
        const playerId = req.query.playerid; 

        if (!playerId) {
            return res.status(400).json({ error: "Player ID is required" });
        }

        // Find the player
        const player = await Player.findOne({ playerid: playerId });

        if (!player) {
            return res.status(404).json({ error: "Failed to find item" });
        }

        // Delete the player
        await Player.findOneAndDelete({ playerid: playerId });

        res.json({ message: "Item Deleted!" });
    } catch (err) {
        console.error(err);
        res.status(500).json({ error: "Server error" });
    }
});
app.listen(3000, ()=>{
    console.log("Running on port 3000");
})
