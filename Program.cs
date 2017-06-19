/* 
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
 * Company: ACME AB								 			             *
 * Programmer: Baran Topal                   							 *
 * WorkspaceName: VideoMerger					 						 *
 * Project Name: VideoMerger           			 					     * 
 * File name: Program.cs                                                 *
 * Version: 1.0                                                          *
 * not super clean code but works                                        * 
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace VideoMerger
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("VideoMerger path_to_video_1 path_to_video_2 path_to_video_n");
                Console.WriteLine("E.g. VideoMerger C:\\video1.mp4 D:\\baz.mp4");
                Console.WriteLine("E.g. VideoMerger C:\\mywedding.avi D:\\yourfunearal.avi");

                Console.WriteLine("Merged video will appear on the desktop with the name as OUTPUT\n");
            }
            else
            {
                var p = new Program();
                p.MergeFiles(args);
                Console.WriteLine("Files are merged!");
            }
        }

        private void MergeFiles(string[] args)
        {
            string strParam;

            // 64-bit
            var Path_FFMPEG = @".\ffmpeg.exe";

            //Merging two videos               
            // var video1 = "C:/Users/Baran.Topal/Desktop/baaa.mp4";
            // var video2 = "C:/Users/Baran.Topal/Desktop/beee.mp4";

            var file = @".\input.txt";

            string[] videos = new string[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                videos[i] = "file " + args[i].Replace(@"\", "/");
            }

            var extension = Path.GetExtension(args[0]);

            // string [] contents = new string[]{"file 'C:\\Users\\Baran.Topal\\Desktop\\baaa.mp4'",
            //      "file 'C:\\Users\\Baran.Topal\\Desktop\\beee.mp4'"};


            File.WriteAllLines(file, videos);
            var userDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string strResult = string.Empty;

            if (File.Exists(userDesktop + "\\OUTPUT" + extension))
            {
                strResult = userDesktop + "\\OUTPUT" + RandomString(3) + extension;
            }
            else
            {
                strResult = userDesktop + "\\OUTPUT" + extension;
            }

            strParam = " -f concat -safe 0 -i " + file + " -c copy " + strResult;

            Process(Path_FFMPEG, strParam);
        }


        public string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void Process(string Path_FFMPEG, string strParam)
        {
            try
            {
                Process ffmpeg = new Process();
                var ffmpeg_StartInfo = new ProcessStartInfo(Path_FFMPEG, strParam);
                ffmpeg_StartInfo.UseShellExecute = false;
                ffmpeg_StartInfo.RedirectStandardError = true;
                ffmpeg_StartInfo.RedirectStandardOutput = true;
                ffmpeg.StartInfo = ffmpeg_StartInfo;
                ffmpeg_StartInfo.CreateNoWindow = true;
                ffmpeg.EnableRaisingEvents = true;
                ffmpeg.Start();
                ffmpeg.WaitForExit(30000);
                //ffmpeg.WaitForExit();
                ffmpeg.Close();
                ffmpeg.Dispose();
                ffmpeg = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
