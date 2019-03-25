﻿using DEM.Net.Lib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Net;
using System.IO.Compression;
using System.Xml;
using System.Diagnostics;
using System.Windows.Media;
using System.Configuration;
using DEM.Net.glTF;
using AssetGenerator.Runtime;
using AssetGenerator;
using System.Numerics;
using Newtonsoft.Json;
using GeoAPI.Geometries;

namespace SampleApp
{
    class Program
    {
        static string _OutputDataDirectory = @"..\..\..\Data";

        [STAThread]
        static void Main(string[] args)
        {

            Logger.StartPerf("Main cold start");

            //DatasetSamples.Run();
            
            //ElevationSamples.Run();

            //TextureSamples textureSamples = new TextureSamples(_OutputDataDirectory);
            //textureSamples.Run();
            //textureSamples.RunImagery(true);

            ReprojectionSamples reprojSamples = new ReprojectionSamples(_OutputDataDirectory, @"..\..\..\Data\GPX\Vernet-les-bains-Canigou-34km.gpx");
            reprojSamples.Run();


            GpxSamples gpxSamples = new GpxSamples( _OutputDataDirectory, @"..\..\..\Data\GPX\Bouleternere-Denivele_de_Noel_2017.gpx");
            gpxSamples.Run();

            OldSamples oldSamples = new OldSamples( _OutputDataDirectory);
            oldSamples.Run();


            Logger.StopPerf("Main cold start", true);
            Console.Write("Press any key to exit...");
            Console.ReadLine();

        }




    }
}