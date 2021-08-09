This is a simple Tekla DTSV NC file parser
==========================================
It currently only supports reading header information, but future plans exists for also reading
the structure parts of the DSTV format and possibly render the DSTV in a SVG browser tag.

Usage :

```C#
            using var streamReader = new StreamReader("[filename]");
            var dstvReader = new DstvReader();

            var parsedDstv = await dstvReader.ParseAsync(streamReader))
            var dstvHeader = parsedDstv.Header; // contains all the header information parsed from the file
```