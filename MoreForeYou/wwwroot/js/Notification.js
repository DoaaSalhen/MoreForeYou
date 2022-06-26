"use strict";
var userId = document.getElementById("userId").value;
var connection = new signalR.HubConnectionBuilder().withUrl("/NotificationHub?userId=" + userId).build();     
connection.on("sendToUser", (type, date, time, benefitId, message, employeeName, employeeUserId) => {

    alert(message);
    var count = parseInt(document.getElementById("notificationCount").textContent);
    document.getElementById("notificationCount").textContent = count + 1;
    var div = document.getElementById("notfiyType");
    var newDiv = document.createElement("div");
    var strong = document.createElement("strong");
    strong.setAttribute("class", "text-info");
    strong.textContent = type;
    newDiv.append(strong);
    var hr = document.createElement("hr");
    newDiv.append(hr);
    var a = document.createElement("a");
    a.setAttribute("asp-action", "UserProfile");
    a.setAttribute("asp-controller", "Employee");
    a.setAttribute("asp-route-userid", userId);
    a.href = "/Employee/UserProfile/?userid=" + employeeUserId;
    var strong2 = document.createElement("strong");
    strong2.setAttribute("class", "text-info");
    strong2.textContent = employeeName;
    a.appendChild(strong2);
    newDiv.append(a);
    var newline = document.createElement("BR");
    div.append(newline);
    div.append(newDiv);
    var divMessage = document.createElement("div");
    divMessage.textContent = message;

    if (type == "Response") {
        var a2 = document.createElement("a");
        a2.setAttribute("asp-route-id", benefitId);
        a2.setAttribute("asp-controller", "Benefit");
        a2.setAttribute("asp-action", "ShowMyBenefitRequests");
        a2.href ="/Benefit/ShowMyBenefitRequests/?BenefitId=" + benefitId;
        var strong3 = document.createElement("strong");
        strong3.textContent = message;
        strong3.setAttribute("class", "text-info");
        a2.appendChild(strong3);
        div.append(a2);
    }
    else
    {
        div.append(divMessage);
    }



    var spanDate = document.createElement("small");
    spanDate.textContent = date +" , ";
    spanDate.setAttribute("class", "text-warning");
    var spanTime = document.createElement("small");
    spanTime.textContent = time;
    spanTime.setAttribute("class", "text-warning");
    //divMessage.append(a2);
    div.append(spanDate);
    div.append(spanTime);


});
connection.start().catch(function (err)
{
    return console.error(err.toString());
}).then(function ()
{
    //document.getElementById("user").value = "UserId: " + userId;
    connection.invoke('GetConnectionId').then(function (connectionId) {
        //document.getElementById('signalRConnectionId').value = connectionId;
    })
});