using System.Diagnostics;

static void Chmod(string path, string mode)
{
    ProcessStartInfo psi = new ProcessStartInfo
    {
        FileName = "/bin/chmod", // On Linux/macOS, chmod is located in /bin/
        Arguments = $"{mode} \"{path}\"",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };
    using (Process process = new Process { StartInfo = psi })
    {
        process.Start();
        process.WaitForExit();
    }
}
static void Chown(string path, string owner, string group)
{
    string userGroup = $"{owner}:{group}";

    ProcessStartInfo psi = new ProcessStartInfo
    {
        FileName = "/bin/chown",  // `chown` command path in Unix-based systems
        Arguments = $" -R {userGroup} \"{path}\"",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    using (Process process = new Process { StartInfo = psi })
    {
        process.Start();
        process.WaitForExit();
    }
}

var OKBLUE="\x1b[94m";
var OKGREEN="\x1b[92m";
var FAIL="\x1b[91m";
var ENDC="\x1b[0m";

Console.Write($"{OKBLUE}Sayt nomini kiriting: {ENDC}");
var name = Console.ReadLine();
Console.Write($"{OKBLUE}Dasturlash tili \n1) dotnet \n2) php \nTanlov: {ENDC}");
var lang=Console.ReadLine();
if(name==""){
    Console.WriteLine($"{FAIL}Bekor qilindi !!!\n{ENDC}");
    Environment.Exit(System.Environment.ExitCode);
}
var path=$"/www/{name}/";
if(Directory.Exists(path+"html")){
    Directory.CreateDirectory(path+"html"); Chmod(path+"html","0777");
}
if(Directory.Exists(path+"log")){
    Directory.CreateDirectory(path+"log"); Chmod(path+"log","0777");
}
if(Directory.Exists(path+"tmp")){
    Directory.CreateDirectory(path+"tmp"); Chmod(path+"tmp","0777");
}
Chown(path,Environment.UserName,"www-data");

Console.WriteLine($"\r\n{OKBLUE}conf faylini to'g'irlash...{ENDC}\r\n");
var conf="";
if(lang=="1"){
    conf=File.ReadAllText("./aspnetcore.conf").Replace("{site_name}",name);
}else{
    conf=File.ReadAllText("./php.conf").Replace("{site_name}",name);
}
File.WriteAllText($"/etc/nginx/sites-available/{name}.conf",conf);
Process.Start($"ln -s /etc/nginx/sites-available/{name}.conf /etc/nginx/sites-enabled/");
Console.WriteLine($"\r\n{OKGREEN}Sayt qoshish ishlari tugadi !!!{ENDC}\r\n");