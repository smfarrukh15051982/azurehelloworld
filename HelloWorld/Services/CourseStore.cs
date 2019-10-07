using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using HelloWorld.Models;

namespace HelloWorld.Services
{
    public class CourseStore
    {
        
        private DocumentClient client;
        private Uri coursesLink;

        public CourseStore()
        {
          
            var uri = new Uri("https://cosmoshelloworld.documents.azure.com:443/");
            var key = "Aq3gCBspDwoARt1nCQnh6kQsDu3IBhSdWPouSwKyvgZb9Bk04ngULUX3A94JTIld90qRRNHrwSC1nGC4fE2jvw==";
            client = new DocumentClient(uri, key);
            coursesLink = UriFactory.CreateDocumentCollectionUri("cosmosdbhelloworld", "courses");
        }

        public async Task InsertCourses(IEnumerable<Course> courses)
        {
            foreach(var course in courses)
            {
                await client.CreateDocumentAsync(coursesLink, course);
            }
        }

        public IEnumerable<Course> GetAllCourses()
        {
            var courses = client.CreateDocumentQuery<Course>(coursesLink)
                                .OrderBy(c => c.Title);

            return courses;
        }
    }
}
