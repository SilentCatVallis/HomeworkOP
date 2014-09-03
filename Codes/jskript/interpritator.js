var a = new Array();
var name;
if (WScript.Arguments.length > 0)
	name = WScript.Arguments(0);
else
{
	WSH.Echo('Enter name of file, please');
	name = WSH.Stdin.ReadLine();
}

var fso = new ActivXObject("Scripting.FileSystemObject");
var O = fso.OpenTextFile(name);
if (fso.FileExist(name) == false)
{
	WScript.StdOut.WriteLine("file not found");
}
else
{
var ip = 0;
var a = O.ReadAll().split(' ');
while (true) {
    if (a[ip] == "exit")
        break;
    if (a[ip] == "put") {
	ip++;
	var rd = a[ip];
	ip++;
	a[a[ip]] = rd;
	ip++;
    }
    if (a[ip] == "swap") {
	var f = a[a[ip + 1]];
	var s = a[a[ip + 2]];
	a[a[ip + 1]] = s;
	a[a[ip + 2]] = f;
	ip += 3;
    }
    if (a[ip] == "read") {
        ip++;
        var rd = a[ip];
        ip++;
        a[a[ip]] = rd;
        ip++;
    }
    if (a[ip] == "write") {

        ip++;
        WSH.echo(a[a[ip]]);
        ip++;
    }
    if (a[ip] == "mins") {

        a[a[ip + 3]] = a[a[ip + 1]] - a[a[ip + 2]];
        ip += 4;
    }

    if (a[ip] == "add") {
        a[a[ip + 3]] = a[a[ip + 1]] + a[a[ip + 2]];
        ip += 4;
    }
    if (a[ip] == "mult") {
        a[a[ip + 3]] = a[a[ip + 1]] * a[a[ip + 2]];
        ip += 4;
    }
    if (a[ip] == "jump") {
        if (a[ip + 1] == 1) {

            ip = a[ip + 2];
        }
        else {

            ip += 3;
        }
    }
    if (a[ip] == "cmp") {

        if (a[a[ip + 1]] == a[a[ip + 2]])
            a[ip + 4] = 1;
        else if (a[a[ip + 1]] > a[a[ip + 2]])
            a[ip + 7] = 1;
        else a[ip + 10] = 1;
        ip += 3;
    }
}
}