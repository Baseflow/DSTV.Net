# DSTV.Net
[![Build and Test](https://github.com/Baseflow/DSTV.Net/actions/workflows/BUILD_TEST_DSTV_NET.yml/badge.svg)](https://github.com/Baseflow/DSTV.Net/actions/workflows/BUILD_TEST_DSTV_NET.yml)
[![Publish](https://github.com/Baseflow/DSTV.Net/actions/workflows/PUBLISH_PACKAGES.yml/badge.svg)](https://github.com/Baseflow/DSTV.Net/actions/workflows/PUBLISH_PACKAGES.yml)
[![NuGet version (DSTV.Net)](https://img.shields.io/nuget/v/DSTV.Net.svg?style=flat-square)](https://www.nuget.org/packages/DSTV.Net/)
[![NuGet version (DSTV.Net.Json)](https://img.shields.io/nuget/v/DSTV.Net.Json.svg?style=flat-square)](https://www.nuget.org/packages/DSTV.Net.Json/)

DSTV.Net is an open-source library tailored for .NET platforms, providing a powerful utility for interacting with DSTV (also known as NC1 or Tekla) files. These files serve as a key industry standard in the steel industry, defining geometry and project information for steel plates.

## Changelog

### 2.0.1

- Added .NET 10 support and retained .NET 8 LTS support.
- Restored `.NET Standard 2.0` support for the core and JSON packages.
- Fixed omitted first `AK` sections when optional header text records are absent.
- Preserved contour weld-preparation angle/blunting values and explicit face codes.
- Added cancellation token overloads throughout parsing.
- Replaced console output with optional `ILogger<DstvReader>` support and a `NullLogger` default.
- Added part feature detection, `PartFeatures`, and order-independent semantic comparison.
- Added source-generated JSON round-trip support in the separate `DSTV.Net.Json` package.
- Added face-filtered, configurable SVG export for all supported blocks.
- Added broad public-format fixtures and coverage for AK/IK/PU/KO/SI/BO/SC/KA records.

## Breaking changes

Version 2.0.0 contains several intentional API changes:

- Supported target frameworks are `.NET Standard 2.0`, `.NET 8`, and `.NET 10`; `.NET 6`, `.NET 7`, and `.NET 9` are not targeted.
- `FlCode` is now the `FaceCode` enum rather than a string.
- `Contour.Type` moved to the common `DstvElement` base for top-level parsed elements.
- `DstvContourPoint` is no longer a `DstvElement`.
- `ToSvg()` was removed from `IDstv` and the model. Use `DstvSvgExporter.Export(...)` or `ExportDocument(...)`.

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

Optional source-generated JSON support is available separately:

```console
> dotnet add <TARGET PROJECT> package DSTV.Net.Json
```

```C#
var json = DstvJson.Serialize((DstvRecord)parsedDstv);
var restored = DstvJson.Deserialize(json);
```

## Header Information Parsing

DSTV.Net offers robust parsing capabilities, adept at extracting header information from DSTV files. This function allows access to crucial metadata such as part name, material type, thickness, and more. Such data can be instrumental in areas like material management and manufacturing planning.

### Usage
```C#
// Open a stream to the dstv file.
using var streamReader = new StreamReader("[filename]");
// Create a new dstv reader
var dstvReader = new DstvReader();
// Parse the dstv file with an optional cancellation token.
using var cancellationSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
var parsedDstv = await dstvReader.ParseAsync(streamReader, cancellationSource.Token);
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


We support this by providing an SVG exporter for one selected profile face. Raw SVG elements and complete SVG documents are available.

SVG's are lightweight and can be used in a variety of applications, including web applications, and are scalable to any size without losing quality.

### Usage
```C#
// Open a stream to the dstv file.
using var streamReader = new StreamReader("[filename]");
// Create a new dstv reader
var dstvReader = new DstvReader();
// Parse the dstv file.
var parsedDstv = await dstvReader.ParseAsync(streamReader));
// Generate SVG for one profile face.
var exporter = new DstvSvgExporter();
exporter[SvgCssClass.Contour].ClassName = "part-outline";
exporter[SvgCssClass.Contour].Style = "fill:none;stroke:navy;stroke-width:1";
var svgElement = exporter.Export(parsedDstv, FaceCode.V);
var svgDocument = exporter.ExportDocument(parsedDstv, FaceCode.V);
// Save a complete SVG file including its doctype.
await File.WriteAllTextAsync("/tmp/dstv.svg", svgDocument).ConfigureAwait(false);
```

See `SvgCssClass` and the exporter's indexer to customize both each generated class name and its raw CSS declarations:

```C#
var exporter = new DstvSvgExporter();
exporter[SvgCssClass.Hole].ClassName = "borehole";
exporter[SvgCssClass.Hole].Style = "fill:none;stroke:darkred;stroke-width:1";
exporter[SvgCssClass.InnerContour].Style = ""; // Keep the class in graphics, but omit its style rule.
```

## AI assistance disclaimer

This version was created using artificial-intelligence assistance.
Such tools may be used to assist with implementation, testing, documentation, and code review in this repository. 
Maintainers remain responsible for evaluating and accepting generated changes. 
AI-assisted output does not replace validation against the DSTV specification, real NC1 exports, or the target machine's postprocessor and production requirements.

Feedback is highly appreciated. Thank you for choosing DSTV.Net - happy coding!