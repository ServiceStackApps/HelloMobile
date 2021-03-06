﻿namespace ServiceModel
{
    public static class Config
    {
        // Windows UWP & WPF Apps can access your server running on localhost as normal
        public const string BaseUrl = "http://localhost:5000";

        // 10.0.2.2 = Android loopback - http://developer.android.com/tools/devices/emulator.html
        public const string UseAndroidLoopback = "http://10.0.2.2:5000";

        // When run from Device Simulator in Remote OSX or external device it needs the IP to your VS.NET Workstation
        // You may also need to open tcp port 5000 in Windows Firewall and enable wildcard IIS Express binding https://stackoverflow.com/a/8569259/85785
        public const string UseNetworkIp = "http://10.0.0.33:5000"; //TODO: replace with your Local Network IP

        public const string ListeningOn = "http://*:5000/";

        public const string PublicKeyXml = "<RSAKeyValue><Modulus>s1/rrg2UxchL5O4yFKCHTaDQgr8Bfkr1kmPf8TCXUFt4WNgAxRFGJ4ap1Kc22rt/k0BRJmgC3xPIh7Z6HpYVzQroXuYI6+q66zyk0DRHG7ytsoMiGWoj46raPBXRH9Gj5hgv+E3W/NRKtMYXqq60hl1DvtGLUs2wLGv15K9NABc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        public const string PrivateKeyXml = "<RSAKeyValue><Modulus>s1/rrg2UxchL5O4yFKCHTaDQgr8Bfkr1kmPf8TCXUFt4WNgAxRFGJ4ap1Kc22rt/k0BRJmgC3xPIh7Z6HpYVzQroXuYI6+q66zyk0DRHG7ytsoMiGWoj46raPBXRH9Gj5hgv+E3W/NRKtMYXqq60hl1DvtGLUs2wLGv15K9NABc=</Modulus><Exponent>AQAB</Exponent><P>6CiNjgn8Ov6nodG56rCOXBoSGksYUf/2C8W23sEBfwfLtKyqTbTk3WolBj8sY8QptjwFBF4eaQiFdVLt3jg08w==</P><Q>xcuu4OGTcSOs5oYqyzsQrOAys3stMauM2RYLIWqw7JGEF1IV9LBwbaW/7foq2dG8saEI48jxcskySlDgq5dhTQ==</Q><DP>KqzhsH13ZyTOjblusox37shAEaNCOjiR8wIKJpJWAxLcyD6BI72f4G+VlLtiHoi9nikURwRCFM6jMbjnztSILw==</DP><DQ>H4CvW7XRy+VItnaL/k5r+3zB1oA51H1kM3clUq8xepw6k5RJVu17GpuZlAeSJ5sWGJxzVAQ/IG8XCWsUPYAgyQ==</DQ><InverseQ>vTLuAT3rSsoEdNwZeH2/JDEWmQ1NGa5PUq1ak1UbDD0snhsfJdLo6at3isRqEtPVsSUK6I07Nrfkd6okGhzGDg==</InverseQ><D>M8abO9lVuSVQqtsKf6O6inDB3wuNPcwbSE8l4/O3qY1Nlq96wWd0DZK0UNqXXdnDQFjPU7uwIH4QYwQMCeoejl3dZlllkyvKVa3jihImDD++qgswX2DmHGDqTIkVABf1NF730gqTmt1kqXoVp5Y+VcO7CZPEygIQyTK4WwYlRjk=</D></RSAKeyValue>";

        public const string FallbackPublicKeyXml = "<RSAKeyValue><Modulus>pj18q4mUIQbF2AT3oQc+ba+vynhg91M+qdpqF2PQ/ud0kdsEbWu5FtP2RvRsuj7blTnBTnZ1yeXUMZKSCLhuKrkqfA1pomGoigiM6stExi/OqZhoKBDJqNt4QZXzNVKrRBPS7GvCYUcm78AmwivSfJN9nF58QunZxHjvmTsnmNcPOOC5+YJDUI0S68v5sYvVhZquvrgmfyhZW1Is8T+AmL32UfOlzktQCFyASfOhYN1gb3/DwGoli41vN5lWoWNbtf/aJFUOwBoTFignE0tey6X5TXcgIZp5HtloIDqgOQBD0xClOpRwYuMwefw6DYP0/fImodq3H/RSTOhtoXspsQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        public const string FallbackPrivateKeyXml = "<RSAKeyValue><Modulus>pj18q4mUIQbF2AT3oQc+ba+vynhg91M+qdpqF2PQ/ud0kdsEbWu5FtP2RvRsuj7blTnBTnZ1yeXUMZKSCLhuKrkqfA1pomGoigiM6stExi/OqZhoKBDJqNt4QZXzNVKrRBPS7GvCYUcm78AmwivSfJN9nF58QunZxHjvmTsnmNcPOOC5+YJDUI0S68v5sYvVhZquvrgmfyhZW1Is8T+AmL32UfOlzktQCFyASfOhYN1gb3/DwGoli41vN5lWoWNbtf/aJFUOwBoTFignE0tey6X5TXcgIZp5HtloIDqgOQBD0xClOpRwYuMwefw6DYP0/fImodq3H/RSTOhtoXspsQ==</Modulus><Exponent>AQAB</Exponent><P>5zZpGMnXoOeGrG2Z5auY3dcUgly5735TBn+1ot5um1x9umHSPAIrazNuteZQyD4bNs7z+0WPkCcUiuvVGbuTgepw644WPO36SMQ5gSsgjttNedGWnD8esHl/Pm1/F+IzHjuU2MZ/rZlyFsRu3C+tXgA1uQjtFnLHF+n5qmBa8Uk=</P><Q>uA/rxcOmWbvri+yDT7f/6iHB+JQDiHQM8OAcjHl5FsEb+OM/2WgOvwtr7BdTsPaaFa6VyXyEnWIdZ1F43V5tj+vwYGO26e+AwDIQAb7ma/1rb7J9LXJ4SH1kta/AL4mr4QjS39+M/0ae2IaBiG6gpufl9d86nMY5qOY5OZIrXSk=</Q><DP>PBaf6ZlLOL3y+gzh2hZmfADRi6+dguhJm37FLbaw+B9pbW7OvFmz/wA23X8lr2S0neHa9op1bPk7FX+EulNNWo4bGpyqmtseGJsmdrNGmtnToL0fbyvYRfTNZOQAC6z1q/3ACTZNKEigpdoXFZIudCeJzrTLKPJbW5OrFuRDvkE=</DP><DQ>iG38o8Tmi8LX0ApKNo+7GA9XmGoVyFHEudJUNudfEremhS/kRsBzlbXgk8milhvjkEis7ADox0NPaiKghO0WJsSKkte2X+XPuCYjaTfX0ZmwxcU2NbaQY6LWQDl6KYJRLWb970TjXOA6o2Hnp3ngiHaBJGMHLedcG84yAnNOwyk=</DQ><InverseQ>WkDNqDhQH8UhxWFq4HCgag0aHNKg7FJfjSk/+u0HsJvH8Q1uibXWYfqoPonebPlG+7u7+i//RdrwYA9vWMC2Tud9j3hguZoP6si6hoA2NyFNGxvNjv8zKIX/b2wwjxB4fDVmEHwz+JTuKjbWf3PtbtmcUUus7HTg7nhgDE96+Ek=</InverseQ><D>AIId3lbleGvhQTmzqZ8AbHyt5oozbrInFgUcT62/EvZxc2w2YWDD0Dtt7HXdGr0sNfK3IfaoAcnlehDTCDqLIK+P/xDZ7rSKe8COsL2WHF6DTN6xy9SQT0c7gQTUuWgjLKo8Wfty3NIHPxKo861HX5jiWI7r5Zb6Mtj1T5RAGN4nMVhG35fMQpY7Tph4km3wr8peR64RaE4JCagwpe5AK/12hISwiLPKOClg3P4ddvQY6oOYZ93qrBQsR8Yg+MSeyfOdxu8GMRnQIbyQJy2luhWKN7EJb758/vJHzGYJFZh5UY/X6FTZbs4Wg66vNH+3WBO/qjZPR96dBmL2NK4cOQ==</D></RSAKeyValue>";

        public const string JwtAuthKeyBase64 = "3n/aJNQHPx0cLu/2dN3jWf0GSYL35QlMqgz+LH3hUyA=";
    }
}