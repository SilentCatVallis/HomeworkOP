var a = new Array ('read', 6, 100, 'read', 30, 101, 'cmp', 100, 101, 'jump', 0, 18, 'jump', 0, 21, 'jump', 0, 28, 'write', 101, 'exit', 'mins', 100, 101, 100, 'jump', 1, 6, 'swap', 100, 101, 'jump', 1, 21)
var ip = 0;
while (true) {
//WSH.echo (a[100], ' ', a[101]);
//WSH.echo('ip=',ip);
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
//WSH.echo ("swap fuck u");
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
//WSH.echo ('doing cmp bleat');
        if (a[a[ip + 1]] == a[a[ip + 2]])
            a[ip + 4] = 1;
        else if (a[a[ip + 1]] > a[a[ip + 2]])
            a[ip + 7] = 1;
        else a[ip + 10] = 1;
        ip += 3;
    }
}