﻿using DEM.Net.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp
{
	class Program
	{
		//public const string tiffPath = @"..\..\..\SampleData\sample.tif";
		//public const string tiffPath = @"..\..\..\SampleData\srtm_38_04.tif"; // from http://dwtkns.com/srtm/ SRTM Tile Grabber
		public const string samplePath = @"..\..\..\SampleData"; // from http://www.opentopography.org/


		static void Main(string[] args)
		{
			string[] files = { "N043E005_AVE_DSM.tif", "N043E006_AVE_DSM.tif", "N044E005_AVE_DSM.tif", "N045E006_AVE_DSM.tif" };
			foreach (var file in files)
			{
				HeightMap heightMap = GetHeightMap(Path.Combine(samplePath, file));
				WriteManifestFiles(heightMap);
				GC.Collect();
			}

			GetGeometryDEM(WKT_GRANDE_BOUCLE, @"..\..\..\SampleData\");
		}

		private static void GetGeometryDEM(string wKT_GRANDE_BOUCLE, string geoTiffRepository)
		{
			throw new NotImplementedException();
		}

		static HeightMap GetHeightMap(string fileName)
		{
			fileName = Path.GetFullPath(fileName);
			string fileTitle = Path.GetFileNameWithoutExtension(fileName);

			HeightMap heightMap = null;
			using (GeoTiff tiffConverter = new GeoTiff(fileName))
			{
				heightMap = tiffConverter.ConvertToHeightMap();
			}
			return heightMap;
		}

		static void WriteManifestFiles(HeightMap heightMap)
		{
			var fileName = heightMap.FileMetadata.Filename;
			var fileTitle = Path.GetFileNameWithoutExtension(fileName);

			// Save metadata
			var outputJsonPath = Path.Combine(Path.GetDirectoryName(fileName), "out ", fileTitle + ".json");
			if (File.Exists(outputJsonPath)) File.Delete(outputJsonPath);

			var bitmapPath = Path.Combine(Path.GetDirectoryName(fileName), "out", fileTitle + ".bmp");
			if (File.Exists(bitmapPath)) File.Delete(bitmapPath);

			// Json manifest
			File.WriteAllText(outputJsonPath, JsonConvert.SerializeObject(heightMap.FileMetadata, Formatting.Indented));

			// Bitmap
			DiagnosticUtils.OutputDebugBitmap(heightMap, bitmapPath);
		}

		static void ImportHeightMapToSqlServer(HeightMap heightMap)
		{
			// Save to SQL
			SqlDemRepository.ClearFileData(heightMap);
			SqlDemRepository.SaveHeightmap(heightMap);
		}

		#region Sample WKT

		private const string WKT_GRANDE_BOUCLE = "LINESTRING(5.4471588134765625 43.54239685275213,5.447598695755005 43.54232686010967,5.448499917984009 43.54240462970736,5.450270175933838 43.54189912552898,5.451364517211914 43.54155693567945,5.451589822769165 43.54143250252535,5.452630519866943 43.5414247254447,5.452491044998169 43.54078700141682,5.453864336013794 43.54060034920576,5.454164743423462 43.541385840026344,5.455001592636108 43.54261460712012,5.455162525177002 43.54268459942852,5.455119609832764 43.54283236070161,5.454883575439453 43.54277014547287,5.453628301620483 43.54329897287061,5.452373027801514 43.54342340217237,5.452415943145752 43.54387445623844,5.452297925949097 43.544457710803165,5.452029705047607 43.54497874677534,5.452061891555786 43.545134279028574,5.4524266719818115 43.545305364043664,5.452308654785156 43.54640185193005,5.45246958732605 43.54694620020958,5.452094078063965 43.54795711968697,5.452297925949097 43.54830704940204,5.451525449752808 43.54905355933778,5.451074838638306 43.549014678840194,5.450838804244995 43.54906133543429,5.4500555992126465 43.54995557984124,5.449733734130859 43.550849810980594,5.449916124343872 43.55092756958308,5.450259447097778 43.55180623481941,5.450087785720825 43.552140590792334,5.449733734130859 43.55249827187253,5.449519157409668 43.553019238343815,5.4480063915252686 43.55332248541017,5.447877645492554 43.5532680565619,5.4470837116241455 43.55270043850059,5.447072982788086 43.55255270141602,5.449637174606323 43.55090424201289,5.449744462966919 43.55084203511484,5.449948310852051 43.55034437761901,5.450077056884766 43.549940027877945,5.450377464294434 43.549574555585714,5.448800325393677 43.54892136554366,5.448875427246094 43.54864142478726,5.448735952377319 43.54834593035609,5.448349714279175 43.54784047599715,5.447491407394409 43.54718726716287,5.44721245765686 43.54707062198327,5.446654558181763 43.5473272410804,5.44622540473938 43.547412780536675,5.4457855224609375 43.54737389898068,5.445528030395508 43.547412780536675,5.445120334625244 43.54584972222492,5.445195436477661 43.54501762987626,5.445624589920044 43.54444215742132,5.445399284362793 43.542715707095056,5.445302724838257 43.5425912763326,5.445420742034912 43.54254461473046,5.445570945739746 43.54261460712012,5.4471588134765625 43.54239685275213)";
		private const string WKT_PETITE_BOUCLE = "LINESTRING(5.44771671295166 43.54234241403722,5.450162887573242 43.54196911866801,5.452415943145752 43.541378062939685,5.452544689178467 43.54070922973245,5.453896522521973 43.54063145794773,5.454990863800049 43.542715707095056,5.453681945800781 43.543275642347915,5.4500555992126465 43.544177749316354,5.448317527770996 43.544768777596964,5.445957183837891 43.54703951656395,5.44546365737915 43.547506096168696,5.445120334625244 43.54526648112833,5.445678234100342 43.544177749316354,5.445399284362793 43.54256016860185,5.44771671295166 43.54234241403722)";
		private const string WKT_GRAND_TRAJET = "LINESTRING(5.447888374328613 43.54240462970736,5.445399284362793 43.5426223840473,5.44546365737915 43.547583859085044,5.439648628234863 43.547537201347275,5.435507297515869 43.55301146275795,5.434112548828125 43.55066318992529,5.427331924438477 43.549792284026665,5.430593490600586 43.544131088942144,5.433168411254883 43.53934043166684,5.426473617553711 43.525899673077,5.42750358581543 43.51326511856049,5.414628982543945 43.50915676244034,5.408105850219727 43.5033672405079,5.406560897827148 43.49925821066224,5.4073333740234375 43.489482576419135,5.402612686157227 43.49838656231855,5.400724411010742 43.488610786964784,5.3936004638671875 43.479207115238765,5.382785797119141 43.48375344394533,5.375232696533203 43.47777463942929,5.371155738830566 43.478989131980235,5.36832332611084 43.47696496416688,5.367679595947266 43.475859428500236,5.368988513946533 43.47336800637108)";

		#endregion
	}
}