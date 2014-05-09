﻿using ServiceStack;

namespace ServiceModel
{
    [Route("/hello")]
    public class Hello : IReturn<HelloResponse>
    {
        public string Name { get; set; }
    }

    public class HelloResponse
    {
        public string Result { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}
