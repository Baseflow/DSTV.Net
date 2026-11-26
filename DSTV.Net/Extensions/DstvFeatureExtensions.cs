using DSTV.Net.Contracts;
using DSTV.Net.Data;
using DSTV.Net.Enums;

namespace DSTV.Net.Extensions;

public static class DstvFeatureExtensions
{
    extension(IDstv model)
    {
        /// <summary>Returns all fabrication features present in the model.</summary>
        public PartFeatures GetPartFeatures()
        {
            var features = PartFeatures.None;

            if (model.HasRegularBoreholes()) features |= PartFeatures.RegularBoreholes;
            if (model.HasSlottedBoreholes()) features |= PartFeatures.SlottedBoreholes;
            if (model.HasPowderMarkings()) features |= PartFeatures.PowderMarkings;
            if (model.HasPunchMarkings()) features |= PartFeatures.PunchMarkings;
            if (model.HasNumerations()) features |= PartFeatures.Numerations;
            if (model.HasBends()) features |= PartFeatures.Bends;
            if (model.HasSawCuts()) features |= PartFeatures.SawCuts;
            if (model.HasSkewedSawCuts()) features |= PartFeatures.SkewedSawCuts;
            if (model.HasRegularCuts()) features |= PartFeatures.RegularCuts;
            if (model.HasSkewedRegularCuts()) features |= PartFeatures.SkewedRegularCuts;
            if (model.HasInternalContours()) features |= PartFeatures.InternalContours;
            if (model.HasCurvedRegularCuts()) features |= PartFeatures.CurvedRegularCuts;

            return features;
        }

        /// <summary>Determines whether the model contains any regular, non-slotted boreholes.</summary>
        public bool HasRegularBoreholes() =>
            model.Elements.OfType<DstvHole>().Any(hole => hole is not DstvSlot);

        /// <summary>Determines whether the model contains any slotted boreholes.</summary>
        public bool HasSlottedBoreholes() =>
            model.Elements.OfType<DstvSlot>().Any();

        /// <summary>Determines whether the model contains any PU powder-marking contour.</summary>
        public bool HasPowderMarkings() =>
            model.Elements.OfType<Contour>().Any(contour => contour.Type == ContourType.PU);

        /// <summary>Determines whether the model contains any KO punch-marking contour.</summary>
        public bool HasPunchMarkings() =>
            model.Elements.OfType<Contour>().Any(contour => contour.Type == ContourType.KO);

        /// <summary>Determines whether the model contains any SI numeration/signing element.</summary>
        public bool HasNumerations() =>
            model.Elements.OfType<DstvNumeration>().Any();

        /// <summary>Determines whether the model contains any KA bend element.</summary>
        public bool HasBends() =>
            model.Elements.OfType<DstvBend>().Any();

        /// <summary>Determines whether the model contains any saw-cut block.</summary>
        public bool HasSawCuts() =>
            model.Elements.OfType<DstvCut>().Any();

        /// <summary>Determines whether the model contains any skewed saw-cut block.</summary>
        public bool HasSkewedSawCuts() =>
            model.Elements.OfType<DstvCut>().Any(cut => !IsStraightSawCut(cut));

        /// <summary>Determines whether the model contains any regular AK/IK contour cut.</summary>
        public bool HasRegularCuts() =>
            model.Elements.OfType<Contour>().Any(contour => IsRegularContour(contour.Type));

        /// <summary>Determines whether an AK/IK contour cut contains skewed/beveled edge data.</summary>
        public bool HasSkewedRegularCuts() =>
            model.Elements.OfType<Contour>()
                .Where(contour => IsRegularContour(contour.Type))
                .SelectMany(contour => contour.Points)
                .OfType<DstvSkewedPoint>()
                .Any(HasBevel);

        /// <summary>Determines whether the model contains an internal contour.</summary>
        public bool HasInternalContours() =>
            model.Elements.OfType<Contour>().Any(contour => contour.Type == ContourType.IK);

        /// <summary>Determines whether the model contains any curved contour segment.</summary>
        public bool HasCurvedRegularCuts() =>
            model.Elements.OfType<Contour>()
                .Where(contour => IsRegularContour(contour.Type))
                .SelectMany(contour => contour.Points)
                .Any(point => point.Radius != 0);
    }

    private static bool IsStraightSawCut(DstvCut cut) =>
        cut.NormalX != 0 && cut.NormalY == 0 && cut.NormalZ == 0;

    private static bool HasBevel(DstvSkewedPoint point) =>
        point.FirstAngle != 0
        || point.FirstBlunting != 0
        || point.SecondAngle != 0
        || point.SecondBlunting != 0;

    private static bool IsRegularContour(ContourType type) =>
        type is ContourType.AK or ContourType.IK;
}
