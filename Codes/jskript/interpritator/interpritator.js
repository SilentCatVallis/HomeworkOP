var a = new Array();
var name;
if (WScript.Arguments.length > 0)
	name = WScript.Arguments(0);
else
{
	WSH.Echo('Enter name of file, please');
	name = WSH.StdIn.ReadLine();
}
var fso = new ActiveXObject("Scripting.FileSystemObject");
if (fso.FileExists(name) == false)
{
	WScript.StdOut.WriteLine("file not found");
}
else
{
	var cmpArr = new array(3);
	var O = fso.OpenTextFile(name);
	var ip = 0;
	var a = O.ReadAll().split(/\s|\r?\n/);
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
		if (a[ip] == "read") {
			var rd = WScript.StdIn.ReadLine();
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
		ip++;
		var t = ip;
		ip++;
		var y = ip;
		ip++;
		var u = ip;
		ip++;
			a[a[u]] = a[a[t]] - a[a[y]];
	
		}
	
		if (a[ip] == "add") {
		ip++
		var t = ip;
		ip++;
		var y = ip;
		ip++;
		var u = ip;
		ip++;
			a[a[u]] = parseInt(a[a[t]], 10) + parseInt(a[a[y]], 10);
		}
		if (a[ip] == "mult") {
		ip++;
		var t = ip;
		ip++;
		var y = ip;
		ip++;
		var u = ip;
		ip++;
			a[a[u]] = a[a[t]] * a[a[y]];
		}
		if (a[ip] == "jump") {
			if (a[parseInt(ip, 10) + 1] == 1) {
				ip = a[parseInt(ip, 10) + 2];
			}
			else {
				ip++; ip++; ip++;
			}
		}
		if (a[ip] == "cmp") {
			cmpArr[0]= 0;
			cmpArr[1]= 0;
			cmpArr[2] = 0;
			if (parseInt(a[a[parseInt(ip, 10) + 1]], 10) == parseInt(a[a[parseInt(ip, 10) + 2]], 10))
				cmpArr[0] = 1;
			else if (parseInt(a[a[parseInt(ip, 10) + 1]], 10) > parseInt(a[a[parseInt(ip, 10) + 2]], 10))
				cmpArr[1] = 1;
			else cmpArr[2] = 1;
			ip++; ip++; ip++;
		}
	}
}