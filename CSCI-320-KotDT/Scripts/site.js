// Write your Javascript code.

function toggleCastList(){
    console.log(document.getElementById('castList'))
    var table = document.getElementById("castList");
    for (var i = 10, row; row = table.rows[i]; i++) { 


        if (row.style.display === "table-row") {
            row.style.display = "none";
        } else {
            row.style.display = "table-row";
        }
         
    } 
}