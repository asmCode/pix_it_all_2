using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace bundle_maker
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Wrong number of parameters");
                return;
            }

            var path = args[0];

            if (!Directory.Exists(path))
            {
                Console.WriteLine(string.Format("Error: Directory {0} doesn't exist.", path));
                return;
            }

            if (!File.Exists(path + "/bundle"))
            {
                Console.WriteLine("Error: No \"bundle\" file inside the directory.");
                return;
            }

            var bundleData = GetDirectoryContentAsBundleData(path);

            var bundleFilename = path + ".pixbundle";
            var bundleCrcFilename = path + ".pixcrc";

            Console.WriteLine("Writtig bundle to " + bundleFilename);

            var memoryStream = new MemoryStream();

            var jsonSerializer = new DataContractJsonSerializer(typeof(BundleData));
            jsonSerializer.WriteObject(memoryStream, bundleData);
            memoryStream.Close();

            File.WriteAllBytes(bundleFilename, memoryStream.ToArray());

            var crc = DamienG.Security.Cryptography.Crc32.Compute(memoryStream.ToArray());
            File.WriteAllText(bundleCrcFilename, crc.ToString());
        }

        static BundleData GetDirectoryContentAsBundleData(string path)
        {
            var bundleData = new BundleData();
            bundleData.BundleMetaData = GetBundleMetaData(path);
            bundleData.Images = GetImagesData(path);

            return bundleData;
        }

        static BundleMetaData GetBundleMetaData(string path)
        {
            var bundleMetaDataInput = LoadFile<BundleMetaDataInput>(path + "/bundle");
            if (bundleMetaDataInput == null)
                return null;

            var bundleMetaData = new BundleMetaData();
            bundleMetaData.Name = bundleMetaDataInput.Name;

            return bundleMetaData;
        }

        static ImageData[] GetImagesData(string path)
        {
            var images = Directory.GetFiles(path, "*.image");
            if (images == null)
                return null;

            var imagesData = new List<ImageData>();

            foreach (var imageFilename in images)
            {
                Console.WriteLine("Processing image file: '{0}'", Path.GetFileName(imageFilename));

                var imageId = Path.GetFileNameWithoutExtension(imageFilename);
                var rawImageData = GetImageRawData(path, imageId);
                if (rawImageData == null)
                {
                    Console.WriteLine("Error: Couldn't load bitmap file for image '{0}'.", imageId);
                    continue;
                }

                var imageDataInput = LoadFile<ImageDataInput>(imageFilename);
                if (imageDataInput == null)
                    continue;
                
                var imageData = new ImageData();
                imageData.Id = imageId;
                imageData.Name = imageDataInput.Name;
                imageData.RawImageData = rawImageData;

                imagesData.Add(imageData);

                Console.WriteLine("Done.");
            }

            return imagesData.ToArray();
        }

        static string GetImageRawData(string path, string imageId)
        {
            var filename = Path.Combine(path, imageId + ".png");

            if (!File.Exists(filename))
                return null;

            var data = File.ReadAllBytes(filename);
            if (data == null)
                return null;

            return Convert.ToBase64String(data);
        }

        static T LoadFile<T>(string path)
            where T : class
        {
            if (!File.Exists(path))
                return null;

            var fileStream = new FileStream(path, FileMode.Open);
            var jsonSerializer = new DataContractJsonSerializer(typeof(T));
            var obj = jsonSerializer.ReadObject(fileStream) as T;

            return obj;
        }
    }
}
