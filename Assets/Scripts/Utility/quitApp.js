#pragma strict

function Start () {
}

function Update () {
    if (Input.GetKey ("escape")) {
        Application.Quit();
    }
}

function  OnApplicationQuit () 
{
	Application.Quit();	
}