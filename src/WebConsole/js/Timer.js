var TimerRunning = false;
var TimerID;
var secs, mins, hours;
var LabelClientID;

function StopTimer() {
    if (TimerRunning) {
        clearTimeout(TimerID);
    }
    TimerRunning = false;
}

function SetUpLabel(labelClientID) {
    LabelClientID = labelClientID;
}

function SetUpTimer(totalSecs) {
    secs = Mod(totalSecs, 60);
    var totalmin = Div(totalSecs, 60);
    mins = Mod(totalmin, 60);
    hours = Div(totalmin, 60);
    $get(LabelClientID).innerHTML = Pad(hours) + ":" + Pad(mins) + ":" + Pad(secs);
}

function StartTimer() {
    TimerRunning = true;
    TickTimer();
}

function TickTimer() {

    TimerID = self.setTimeout("TickTimer()", 1000);

    if (secs == 60) {
        mins++;
        secs = 0;
    }

    if (mins == 60) {
        hours++;
        mins = 0;
    }

    $get(LabelClientID).innerHTML = Pad(hours) + ":" + Pad(mins) + ":" + Pad(secs);

    secs++;
}

function Pad(number) //pads the mins/secs with a 0 if its less than 10 
{
    if (number < 10) {
        number = 0 + "" + number;
    }
    return number;
}

function Mod(X, Y) { return X - Math.floor(X / Y) * Y }

function Div(X, Y) { return Math.floor(X / Y) }