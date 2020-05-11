using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace appsettingstransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                #region release

                if (args.Length < 3)
                {
                    Console.WriteLine("Missing Arguments. Expects 3 arguments option, sourcefile, dockersecret");
                }
                else
                {
                    var option = args[0] ?? string.Empty;
                    var sourcefile = args[1] ?? string.Empty;
                    var dockerconfig = args[2] ?? string.Empty;

                    if (sourcefile.Length > 0 && dockerconfig.Length > 0 && option.Length > 0)
                    {
                        if (Options.isValidOption(option))
                            execute(option, sourcefile, dockerconfig);
                        else
                            Console.WriteLine("Valid Options {0}, {1}, {2} and {3} ", Options.apppoolsetting, Options.createlocalaccount, Options.enablegzip, Options.transformconfig);
                    }
                    else
                        Console.WriteLine("NOTHING TO DO HERE");
                }

                #endregion release

                #region debug

                //if (args.Length > 3)
                //{
                //    Console.WriteLine("Missing Arguments. Expects 3 arguments option, sourcefile, dockersecret");
                //}
                //else
                //{
                //    var option = Options.transformconfig;
                //    var sourcefile = @"E:\Git\HammerLane-AccountManagement\Api\Xpo.LastMile.AccountManagement.Backend.Api.Site\Web.Release.config";
                //    //var dockerconfig = @"E:\Git\Hammerlane-Central-Configs\configs\Demo.json";
                //    var dockerconfig = @"E:\Git\Hammerlane-Central-Configs\configs\Perf.json";

                //    if (sourcefile.Length > 0 && dockerconfig.Length > 0 && option.Length > 0)
                //    {
                //        if (Options.isValidOption(option))
                //            execute(option, sourcefile, dockerconfig);
                //        else
                //            Console.WriteLine("Valid Options {0}, {1}, {2} and {3} ", Options.apppoolsetting, Options.createlocalaccount, Options.enablegzip, Options.transformconfig);
                //    }
                //    else
                //        Console.WriteLine("NOTHING TO DO HERE");
                //}


                #endregion debug
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void execute(string option, string sourcefile, string dockersecret)
        {
            switch (option)
            {
                case Options.transformconfig:
                    if (sourcefile.Length > 0 && dockersecret.Length > 0)
                    {
                        TransformConfigs.transform(sourcefile, dockersecret);
                    }
                    else
                        Console.WriteLine("NOT SURE WHAT TO DO HERE");
                    break;                
                case Options.createlocalaccount:
                    if (dockersecret.Length > 0)
                    {
                        string username = string.Empty;
                        string password = string.Empty;
                        TransformConfigs.getdockersecret(dockersecret, out username, out password);
                        if (username.Length > 0 && password.Length > 0)
                            ContainerService.createLocalAdmin(username, password);
                        else
                            Console.WriteLine("NOT SURE WHAT TO DO HERE");
                    }
                    else
                        Console.WriteLine("NOT SURE WHAT TO DO HERE");
                    break;                
                default:
                    if (sourcefile.Length > 0 && dockersecret.Length > 0)
                        execute(Options.transformconfig, sourcefile, dockersecret);
                    break;
            }
        }

        
    }
}
