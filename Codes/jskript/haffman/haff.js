if (WScript.Arguments.length != 3)
{
	WSH.echo("incorrect data, please try again");
	WScript.Quit();
}
var name = WScript.Arguments(0),
	input = WScript.Arguments(1),
	output = WScript.Arguments(2),
	fso = new ActiveXObject("Scripting.FileSystemObject");
if (fso.FileExists(input) == false)
{
	WScript.StdOut.WriteLine("file not found, please try again");
	WScript.Quit();
}
var	file = fso.OpenTextFile(input.toString()),
	inputData = file.ReadAll();
file.Close();

function Node (left, right, letter, weight)
{
	this.left = left;
	this.right = right;
	this.letter = letter;
	this.weight = weight;
}

function cmp (q, w)
{
	if (q.weight < w.weight)
		return -1;
	if (q.weight > w.weight)
		return 1;
	return 0;
}

function findCode (a, str, myLetter)
{
	var ans = '';
	if (a.letter.length == 1)
	{
		return str.toString();
	}
	else
	{
		if (a.left.letter.indexOf(myLetter) != -1)
		{
			str = str.concat('1');
			return findCode(a.left, str, myLetter);
		}
		else
		{
			str = str.concat('0');
			return findCode(a.right, str, myLetter);
		}
	}
}

if (name == 'encode')
{
	var alphabet = new Array();
	var n = 0;
	var count = 0;
	
	for (var i = 0; i < inputData.length; ++i)
	{
		count++;
		if (inputData.charAt(i) in alphabet)
			alphabet[inputData.charAt(i)]++;
		else
		{
			alphabet[inputData.charAt(i)] = 1;
			n++;
		}
	}
	
	var en = 0;
	if (n == 1)
	{
		for (var i in alphabet)
		{
			if (alphabet[i] == 1)
				en = 0;
			else
				en = 1;
		}		
	}
	else
	{
		for (var i in alphabet)
		{
			en = en + (Math.log(alphabet[i] / count) / Math.log(n)) * (alphabet[i] / count);
		}
		en *= -1;
	}
	
	var c = new Array();
	var k = 0;
	for (var i in alphabet)
	{
		c[k] = new Node (null, null, i, alphabet[i]);
		k++;
	}
	
	var j = 1;
	
	while (c.length > 1)
	{
		c.sort(cmp);
		var newNode = new Node(c[0], c[1], c[0].letter.toString() + c[1].letter.toString(), (+c[0].weight) + (+c[1].weight));
		c.shift();
		c[0] = newNode;
	}
	var answer = "";
	for (var i = 0; i < inputData.length; ++i)
	{
		answer = answer.concat(findCode(c[0], "", inputData.charAt(i)));
	}
	//WSH.echo(answer, en);
	
	var count1 = 0;
	var count0 = 0;
	for (var i = 0; i < answer.length; ++i)
	{
		if (answer.charAt(i) == '1')
			count1++;
		else
			count0++;
	}
	var en2 = 0;
	if (count1 == 0 || count0 == 0)
	{
		if (answer.length == 1)
			en2 = 0;
		else
			en2 = 1;
	}
	else
	{
		en2 = en2 + (Math.log(count0 / answer.length) / Math.log(2)) * (count0 / answer.length);
		en2 = en2 + (Math.log(count1 / answer.length) / Math.log(2)) * (count1 / answer.length);
		en2 *= -1;
	}

	
	var out = fso.OpenTextFile(output.toString(), 2, -2);
	out.Write(answer);
	out.Close();
	
	
	var tree = new Array();
	var l = 0;
	function MadeTree(q)
	{
		tree[l] = q.letter;
		l++;
		tree[l] = q.weight;
		l++;
		if (q.left != null)
		{
			tree[l] = 1;
			l++;
			MadeTree(q.left);
		}
		else
		{
			tree[l] = 0;
			l++;
		}
		
		if (q.right != null)
		{
			tree[l] = 1;
			l++;
			MadeTree(q.right);
		}
		else
		{
			tree[l] = 0;
			l++;
		}
	}
	MadeTree(c[0]);
	
	var wall = '#%';
	var mayBeTree = tree.toString();
	var pos = mayBeTree.indexOf(wall);
	if (pos != -1)
		wall = '%#';
	
	
	tree = tree.join(wall);
	//WSH.echo(tree);
	tree = wall.concat(tree);
	var treeFile = fso.OpenTextFile("tree.txt", 2, -2);
	treeFile.Write(tree);
	treeFile.Close();
	
	WSH.echo("last entropia", en);
	var memory1 = inputData.length * 8;
	WSH.echo("last memory = ", memory1, " bits");
	
	
	WSH.echo("new entropia", en2);
	memory2 = answer.length;
	WSH.echo("new memory = ", memory2, " bits");
	
	WSH.echo("last memory / new memory = ", memory1/memory2);
}
//////////////////////
//////////////////////



else if (name == 'decode')
{
	var treeIn = fso.OpenTextFile("tree.txt");
	var tree1 = treeIn.ReadAll();
	var ip = 0;
	var InFileNew = fso.OpenTextFile(input.toString());
	var answer = InFileNew.ReadAll();

	var wall = '';
	wall = wall.concat(tree1.charAt(0), tree1.charAt(1));
	var tree = new Array();
	
	tree = tree1.split(wall);
	var l = 1;
	var nd = new Node(null, null, "", 0);
	function ParseTree(q)
	{
		q.letter = tree[l];
		l++;
		q.weight = tree[l];
		l++;
		if (tree[l] == 1)
		{
			l++;
			q.left = new Node(null, null, "", 0);
			ParseTree(q.left);
		}
		else
		{
			l++;
			q.left = null;
		}
		
		if (tree[l] == 1)
		{
			l++;
			q.right = new Node(null, null, "", 0);
			ParseTree(q.right);
		}
		else
		{
			l++;
			q.right = null;
		}
	}	
	ParseTree(nd);
	function findLetter (s)
	{
		if (s.letter.length == 1)
			return s.letter;
		else
		{
			if (answer.charAt(ip) == '1')
			{
				ip++;
				return findLetter(s.left);
			}
			else
			{
				ip++;
				return findLetter(s.right);
			}
		}
	}

	var answer2 = '';
	for (var i = 0; i < nd.weight; ++i)
	{
		answer2 = answer2.concat(findLetter(nd));
	}

	var NewOutFile = fso.OpenTextFile(output.toString(), 2, -2);
	NewOutFile.Write(answer2);
	NewOutFile.Close();
}
else
{
	WSH.echo("incorrect data, please try again");
	WScript.Quit();
}

