const itemContainer = document.getElementById("users-container");

const fetchUsers = async()=>{
    try
    {
        const response = await fetch("/playerLeaderBoard");
        if(!response.ok)
        {
            throw new Error("Failed to get Items");
        }
        //parse json
        const items = await response.json();
        //format data to html
        itemContainer.innerHTML = "";
        items.forEach((player) => {
            const itemDiv = document.createElement("div");
            const updateButton = document.createElement("button");
            const deleteButton = document.createElement("button");

            itemDiv.className = "item";
            itemDiv.innerHTML = `<br>ID: ${player.playerid}<br>Player: ${player.name}<br> Score: ${player.score}<br> Wins:  ${player.wins}<br>`;               
            
            updateButton.data = player.playerid;
            updateButton.innerHTML = 'Update';
            deleteButton.data = player.playerid;
            deleteButton.innerHTML = 'Delete';
            updateButton.onclick = function() {
                window.location.href = `update.html?playerid=${player.playerid}&name=${player.name}&score=${player.score}&timesPlayed=${player.timesPlayed}&wins=${player.wins}`;
            }
            
            // Delete button logic
            deleteButton.onclick = async function() {
                const response = await fetch(`/deletePlayerUnity?playerid=${player.playerid}`, {
                    method: 'DELETE',
                });
                const mes = await response.json();
                alert(mes.message);
                window.location.href = '/users.html';
            }
            
            itemContainer.appendChild(itemDiv);
            itemContainer.appendChild(updateButton);
            itemContainer.appendChild(deleteButton);
        });
    }
    catch(error)
    {
        console.error("Error: ", error);
        itemContainer.innerHTML = "<p style='color:red'>Failed to get items</p>"
    }
};

fetchUsers();