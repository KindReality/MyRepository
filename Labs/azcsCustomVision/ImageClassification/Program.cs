﻿using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace ImageClassification
{
    class Program
    {
        private static List<string> hemlockImages;
        private static List<string> japaneseCherryImages;
        private static MemoryStream testImage;

        static void Main(string[] args)
        {
            //don't forget the other project
            //don't forget the custom vision portal https://www.customvision.ai/projects
            //use the custom vision portal to navigate to a project and unpublish
            // Add your Azure Custom Vision subscription key and endpoint to your environment variables.
            // <snippet_endpoint>
            string ENDPOINT = "https://stcustomvision-prediction.cognitiveservices.azure.com/";
            // </snippet_endpoint>

            // <snippet_keys>
            // Add your training & prediction key from the settings page of the portal
            string trainingKey = "76fdb20ae1eb49e28733221e202909a5";
            string predictionKey = "74ad2b8d75fa4e4db72f23149516bcdd";
            // </snippet_keys>

            // <snippet_create>
            // Create the Api, passing in the training key
            CustomVisionTrainingClient trainingApi = new CustomVisionTrainingClient()
            {
                ApiKey = trainingKey,
                Endpoint = ENDPOINT
            };

            // Create a new project
            Console.WriteLine("Creating new project:");
            var project = trainingApi.CreateProject("My New ImageClassification Project");
            // </snippet_create>

            // <snippet_tags>
            // Make two tags in the new project
            var hemlockTag = trainingApi.CreateTag(project.Id, "Hemlock");
            var japaneseCherryTag = trainingApi.CreateTag(project.Id, "Japanese Cherry");
            // </snippet_tags>

            // <snippet_upload>
            // Add some images to the tags
            Console.WriteLine("\tUploading images");
            LoadImagesFromDisk();

            // Images can be uploaded one at a time
            foreach (var image in hemlockImages)
            {
                using (var stream = new MemoryStream(File.ReadAllBytes(image)))
                {
                    trainingApi.CreateImagesFromData(project.Id, stream, new List<Guid>() { hemlockTag.Id });
                }
            }
            // </snippet_upload>

            // Or uploaded in a single batch 
            var imageFiles = japaneseCherryImages.Select(img => new ImageFileCreateEntry(Path.GetFileName(img), File.ReadAllBytes(img))).ToList();
            trainingApi.CreateImagesFromFiles(project.Id, new ImageFileCreateBatch(imageFiles, new List<Guid>() { japaneseCherryTag.Id }));

            // <snippet_train>
            // Now there are images with tags start training the project
            Console.WriteLine("\tTraining");
            var iteration = trainingApi.TrainProject(project.Id);

            // The returned iteration will be in progress, and can be queried periodically to see when it has completed
            while (iteration.Status == "Training")
            {
                Thread.Sleep(1000);

                // Re-query the iteration to get it's updated status
                iteration = trainingApi.GetIteration(project.Id, iteration.Id);
            }

            // The iteration is now trained. Publish it to the prediction end point.
            var publishedModelName = "treeClassModel";
            var predictionResourceId = "/subscriptions/a0980536-0ea6-4373-9e3a-abdce75f3c47/resourceGroups/SaftTechnologies/providers/Microsoft.CognitiveServices/accounts/stCustomVision-Prediction";
            trainingApi.PublishIteration(project.Id, iteration.Id, publishedModelName, predictionResourceId);
            Console.WriteLine("Done!\n");
            // </snippet_train>

            // Now there is a trained endpoint, it can be used to make a prediction

            // <snippet_prediction_endpoint>
            // Create a prediction endpoint, passing in obtained prediction key
            CustomVisionPredictionClient endpoint = new CustomVisionPredictionClient()
            {
                ApiKey = predictionKey,
                Endpoint = ENDPOINT
            };
            // </snippet_prediction_endpoint>

            // <snippet_prediction>
            // Make a prediction against the new project
            Console.WriteLine("Making a prediction:");
            var result = endpoint.ClassifyImage(project.Id, publishedModelName, testImage);

            // Loop over each prediction and write out the results
            foreach (var c in result.Predictions)
            {
                Console.WriteLine($"\t{c.TagName}: {c.Probability:P1}");
            }
            Console.ReadKey();
            // </snippet_prediction>
        }

        private static void LoadImagesFromDisk()
        {
            // this loads the images to be uploaded from disk into memory
            hemlockImages = Directory.GetFiles(Path.Combine("Images", "Hemlock")).ToList();
            japaneseCherryImages = Directory.GetFiles(Path.Combine("Images", "Japanese Cherry")).ToList();
            testImage = new MemoryStream(File.ReadAllBytes(Path.Combine("Images", "Test\\test_image.jpg")));
        }
    }
}
