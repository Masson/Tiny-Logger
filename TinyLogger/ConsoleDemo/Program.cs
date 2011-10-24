using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMasson.Util.TinyLogger;
using IMasson.Util.TinyLogger.Recorder;
using System.Threading;

namespace ConsoleDemo
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Log logger = Log.GetInstance();
            PlainFileLogRecorder fileRecorder = new PlainFileLogRecorder();
            ConsoleLogRecorder consoleRecorder = new ConsoleLogRecorder();
            MultipleLogRecorder recorder = new MultipleLogRecorder();
            recorder.AddRecorder(fileRecorder);
            recorder.AddRecorder(consoleRecorder);

            //WindowsEventLogRecorder recorder = new WindowsEventLogRecorder();

            logger.SetRecorder(LogLevel.Verbose, recorder);
            logger.SetRecorder(LogLevel.Debug, recorder);
            logger.SetRecorder(LogLevel.Info,     recorder);
            logger.SetRecorder(LogLevel.Warning,  recorder);
            logger.SetRecorder(LogLevel.Error,    recorder);

            Log.V("test1", "hello world");
            Thread.Sleep(1000);
            Log.D("test1", "Debug message");
            Thread.Sleep(1000);
            Log.I("test2", "Information message");
            Thread.Sleep(1000);
            Log.W("test2", "Warning message");
            Thread.Sleep(1000);
            Log.E("test2", "Error message");
            Thread.Sleep(1000);

            logger.Shutdown();

            System.Console.WriteLine("-" + PlainFileLogRecorder.DefaultFilePath + "-");
            System.Console.ReadLine();
        }
    }
}
