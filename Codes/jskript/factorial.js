var a = new Array ('read', 6, 100, 'put', 1, 105, 'put', 1, 101, 'put', 0, 102, 'cmp', 100, 102, 'jump', 0, 24, 'jump', 0, 27, 'jump', 0, 0, 'write', 101, 'exit', 'mult', 100, 101, 101, 'mins', 100, 105, 100, 'jump', 1, 12)
var ip = 0;
while (true) {
//WSH.echo (a[100], ' ', a[101], ' ', a[102]);
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
//WSH.echo('write');
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
//WSH.echo('jump true');
            ip = a[ip + 2];
        }
        else {
//WSH.echo('jump false');
            ip += 3;
        }
    }
    if (a[ip] == "cmp") {
//WSH.echo('cmp');
        if (a[a[ip + 1]] == a[a[ip + 2]])
            a[ip + 4] = 1;
        else if (a[a[ip + 1]] > a[a[ip + 2]])
            a[ip + 7] = 1;
        else a[ip + 10] = 1;
        ip += 3;
    }
}