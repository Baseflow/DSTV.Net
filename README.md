# DSTV.Net
[![Build and Test](https://github.com/Baseflow/DSTV.Net/actions/workflows/BUILD_TEST_DSTV_NET.yml/badge.svg)](https://github.com/Baseflow/DSTV.Net/actions/workflows/BUILD_TEST_DSTV_NET.yml)
[![Publish](https://github.com/Baseflow/DSTV.Net/actions/workflows/PUBLISH_PACKAGES.yml/badge.svg)](https://github.com/Baseflow/DSTV.Net/actions/workflows/PUBLISH_PACKAGES.yml)
[![NuGet version (DSTV.Net)](https://img.shields.io/nuget/v/DSTV.Net.svg?style=flat-square)](https://www.nuget.org/packages/DSTV.Net/)

DSTV.Net is an open-source library tailored for .NET platforms, providing a powerful utility for interacting with DSTV (also known as NC1 or Tekla) files. These files serve as a key industry standard in the steel industry, defining geometry and project information for steel plates.
Introduction

DSTV.Net is a precision-oriented, high-performance library designed to simplify the process of working with DSTV files. It equips developers and software applications with the ability to read and interpret DSTV file content effortlessly, making it ideal for CAD, CAM, CNC, and other software applications associated with steel fabrication and structural engineering.
Features

## Getting started

Installation via Package Manager Console in Visual Studio:

```powershell
PM> Install-Package DSTV.Net
```

Installation via .NET CLI:

```console
> dotnet add <TARGET PROJECT> package DSTV.Net
```

## Header Information Parsing

DSTV.Net offers robust parsing capabilities, adept at extracting header information from DSTV files. This function allows access to crucial metadata such as part name, material type, thickness, and more. Such data can be instrumental in areas like material management and manufacturing planning.

### Usage
```C#
// Open a stream to the dstv file.
using var streamReader = new StreamReader("[filename]");
// Create a new dstv reader
var dstvReader = new DstvReader();
// Parse the dstv file.
var parsedDstv = await dstvReader.ParseAsync(streamReader))
// Access the header information
var dstvHeader = parsedDstv.Header;
// Print the order identification
Console.WriteLine(dstvHeader.OrderIdentification);
``` 

The header information contains the following properties:

| Property | Type | Description                                                                |
| --- | --- |----------------------------------------------------------------------------------|
| OrderIdentification | string | The order identification of the project                       |
| DrawingIdentification | string | The drawing identification of the piece within the project  |
| PhaseIdentification | string | The phase identification of the piece within the project      |
| PieceIdentification | string | The piece identification of the piece                         |
| SteelQuality | string | The steel quality of the piece                                       |
| QuanityOfPieces | int | The quantity of pieces in the project                                |
| Profile | string | The profile of the piece                                                  |
| CodeProfile | CodeProfile | The code profile of the piece                                    |
| Length | double | The length of the piece                                                    |
| SawLength | double | The saw length of the piece                                             |
| ProfileHeight | double | The profile height of the piece                                     |
| FlangeWidth | double | The flange width of the piece                                         |
| WebThickness | double | The web thickness of the piece                                       |
| Radius | double | The radius of the piece                                                    |
| WeightByMeter | double | The weight by meter of the piece                                    |
| PaintingSurfaceByMeter | double | The painting surface by meter of the piece                 |
| WebStartCut | double | The web start cut of the piece                                        |
| WebEndCut | double | The web end cut of the piece                                            |
| FlangeStartCut | double | The flange start cut of the piece                                  |
| FlangeEndCut | double | The flange end cut of the piece                                      |
| Text1InfoOnPiece | string | The text 1 info on the piece                                     |
| Text2InfoOnPiece | string | The text 2 info on the piece                                     |
| Text3InfoOnPiece | string | The text 3 info on the piece                                     |
| Text4InfoOnPiece | string | The text 4 info on the piece                                     |

The CodeProfile contains the following values:

| Code | Description |
| --- | --- |
| I | Profile I |
| L | Profile L |
| U | Profile U |
| B | Sheets, Plate, teared sheets, etc. |
| RU | Round |
| RO | Rouned Tube |
| M | Rectangular Tube |
| C | Profile C |
| T | Profile T |
| SO | Special Profile |

## Geometry Visualization

In addition to parsing, DSTV.Net also facilitates the visualization of the geometry of steel plates as defined in DSTV files. It achieves this by converting the geometric data into an SVG (Scalable Vector Graphics) format. This immediate visual representation aids better understanding and verification of the data extracted.

![image](./.img/dstv.svg)
![image](./.img/dstv2.svg)


We support this by providing a SVG generator that can be used to generate SVG files from the parsed DSTV data.

SVG's are lightweight and can be used in a variety of applications, including web applications, and are scalable to any size without losing quality.

### Usage
```C#
// Open a stream to the dstv file.
using var streamReader = new StreamReader("[filename]");
// Create a new dstv reader
var dstvReader = new DstvReader();
// Parse the dstv file.
var parsedDstv = await dstvReader.ParseAsync(streamReader));
// Generate the SVG content
var svg = parsedDstv.ToSvg();
// Save the svg to a file
await File.WriteAllTextAsync("/tmp/dstv.svg",  @"<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.1//EN"" ""http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd"">" + Environment.NewLine + svg).ConfigureAwait(false);
```

Feedback is highly appreciated. Thank you for choosing DSTV.Net - happy coding!