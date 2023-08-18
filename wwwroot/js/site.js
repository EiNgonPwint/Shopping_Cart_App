
window.onload = function () {
    initListeners();


}

var cart_count = 0;
function initListeners() {
    // setup each node to listen to a mouse-click
    let buttonsAdd = document.getElementsByClassName("AddtoCartBtn");
    for (let i = 0; i < buttonsAdd.length; i++) {
        buttonsAdd[i].addEventListener('click', onAddButtonClick);
    }
    let buttonsRemove = document.getElementsByClassName("RemovefmCartBtn");
    for (let i = 0; i < buttonsRemove.length; i++) {
        buttonsRemove[i].addEventListener('click', onRemoveButtonClick);
    }
    let stars = document.getElementsByClassName("ratings_stars");
    for (let i = 0; i < stars.length; i++) {
        stars[i].addEventListener('click', onStarClick);
    }
}
function onAddButtonClick(event) {

    AddTo(event.target.id);
    IncreaseQty(event.target.id);
    Subtotal(event.target.id);
    TotalCost();
    //GetProductQty(event.target.id);
    //dbGetCount();
}

function onRemoveButtonClick(event) {

    RemoveFrom(event.target.id);
    DecreaseQty(event.target.id);
    Subtotal(event.target.id);
    TotalCost();
    //GetProductQty(event.target.id);
    //dbGetCount();
}
function onStarClick(event) {

    SendRating(event.target.id);
}
function SendRating(id) {


    let ajax = new XMLHttpRequest();

    
    ajax.open("POST", "/Rating/Index");
    ajax.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

    ajax.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
               
                return this.responseText;
            }
        }
    }

    // key-value pairs
    ajax.send("id=" + id)

}



//function onButtonClick(event) {

//    AddTo(event.target.id);
//    //dbGetCount();
//}


//function onGetNodeColors(data) {
//    if (data == null) {
//        return;
//    }

//    for (let i = 0; i < data.length; i++) {
//        let entry = data[i];

//        let node = document.getElementById(entry.NodeId);
//        if (node != null) {
//            node.style.background = entry.Color;
//        }
//    }
//}
