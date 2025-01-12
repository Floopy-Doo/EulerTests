using System.Numerics;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Tests;

public class QuaternionConversionTests
{
    /// Initially generated with ChatGPT then post computed with wolfram alpha knowledge
    /// -> Calculated with WolframAlpha -> using 3-2-1 (Yaw, Pitch, Roll or Z, Y, X)
    /// -> Quartion maped to β_0 = w, β_1 = y, β_2 = x, β_3 = z
    public static TheoryData<Vector3, Quaternion> TestCasesNormalConversion => new()
    {
        { new Vector3(0, 0, 0), new Quaternion(0.0000f, 0.0000f, 0.0000f, 1.0000f) },
        { new Vector3(90, 0, 0), new Quaternion(0.0000f, 0.7071f, 0.0000f, 0.7071f) },
        { new Vector3(0, 90, 0), new Quaternion(0.7071f, 0.0000f, 0.0000f, 0.7071f) },
        { new Vector3(0, 0, 90), new Quaternion(0.0000f, 0.0000f, 0.7071f, 0.7071f) },
        { new Vector3(45, 45, 45), new Quaternion(0.4619f, 0.1913f, 0.1913f, 0.8446f) },
        { new Vector3(180, 0, 0), new Quaternion(0.0000f, 1.0000f, 0.0000f, 0.0000f) },
        { new Vector3(0, 180, 0), new Quaternion(1.0000f, 0.0000f, 0.0000f, 0.0000f) },
        { new Vector3(0, 0, 180), new Quaternion(0.0000f, 0.0000f, 1.0000f, 0.0000f) },
        { new Vector3(90, 90, 0), new Quaternion(0.5000f, 0.5000f, -0.5000f, 0.5000f) },
        { new Vector3(90, 0, 90), new Quaternion(0.5000f, 0.5000f, 0.5000f, 0.5000f) },
        { new Vector3(0, 90, 90), new Quaternion(0.5000f, -0.5000f, 0.5000f, 0.5000f) },
        { new Vector3(45, 0, 0), new Quaternion(0.0000f, 0.3827f, 0.0000f, 0.9239f) },
        { new Vector3(0, 45, 0), new Quaternion(0.3827f, 0.0000f, 0.0000f, 0.9239f) },
        { new Vector3(0, 0, 45), new Quaternion(0.0000f, 0.0000f, 0.3827f, 0.9239f) },
        { new Vector3(30, 60, 90), new Quaternion(0.5000f, -0.1830f, 0.5000f, 0.6830f) },
        { new Vector3(60, 30, 90), new Quaternion(0.5000f, 0.1830f, 0.5000f, 0.6830f) },
        { new Vector3(90, 30, 60), new Quaternion(0.5000f, 0.5000f, 0.1830f, 0.6830f) },
        { new Vector3(60, 90, 30), new Quaternion(0.6830f, 0.1830f, -0.1830f, 0.6830f) },
        { new Vector3(30, 90, 60), new Quaternion(0.6830f, -0.1830f, 0.1830f, 0.6830f) },
        { new Vector3(90, 60, 30), new Quaternion(0.5000f, 0.5000f, -0.1830f, 0.6830f) },
    };

    public static TheoryData<Vector3, Quaternion> TestCasesGimbalLock => new()
    {
        { new Vector3(90, 90, 90), new Quaternion(0.7071f, 0.0000f, 0.7071f, 0.0000f) },
        { new Vector3(-90, 90, 90), new Quaternion(0.0000f, 0.0000f,0.0000f, 0.0000f) },
        { new Vector3(90, -90, 90), new Quaternion(0.7071f, -0.7071f, 0.7071f, 0.7071f) },
        { new Vector3(90, 90, -90), new Quaternion(0.7071f, 0.7071f, -0.7071f, 0.7071f) },
        { new Vector3(-90, -90, 90), new Quaternion(-0.7071f, -0.7071f, 0.7071f, 0.7071f) },
        { new Vector3(90, -90, -90), new Quaternion(0.7071f, -0.7071f, -0.7071f, 0.7071f) },
        { new Vector3(-90, 90, -90), new Quaternion(-0.7071f, 0.7071f, -0.7071f, 0.7071f) },
        { new Vector3(-90, -90, -90), new Quaternion(-0.7071f, -0.7071f, -0.7071f, 0.7071f) },
        { new Vector3(180, 90, 90), new Quaternion(0.7071f, 0.7071f, 0.0000f, 0.0000f) },
        { new Vector3(90, 180, 90), new Quaternion(0.7071f, 0.0000f, 0.7071f, 0.0000f) }
    };

    /// Calculated with WolframAlpha -> using 3-2-1 (Yaw, Pitch, Roll or Z, Y, X)
    /// Quartion maped to β_0 = w, β_1 = y, β_2 = x, β_3 = z
    /// Wolfram alpha has ben replaced https://www.andre-gaschler.com/rotationconverter/ an mappig matches
    public static TheoryData<Vector3, Quaternion> TestCasesWolframAlpha => new()
    {
        { new Vector3(30, 15, 20), new Quaternion(0.1687f, 0.2308f, 0.1330f, 0.9490f) },
        { new Vector3(67, 48, 21), new Quaternion(0.4253f, 0.4340f, -0.0819f, 0.7900f) },
        { new Vector3(0, 0, 0), new Quaternion(0.0000f,0.0000f,0.0000f,1.0000f) },
        { new Vector3(45, 0, 0), new Quaternion(0.0000f,0.3826f,0.0000f,0.9238f) },
    };

    /// Calculated with https://www.andre-gaschler.com/rotationconverter/ -> using XYZ (Yaw, Pitch, Roll)
    /// Same result may be achieved using https://tools.glowbuzzer.com/rotationconverter -> with exception to -90,0,0 and 270,0,0
    public static TheoryData<Vector3, Quaternion> TestCasesAndreGaschler => new()
    {
        { new Vector3(0, 0, 0), new Quaternion(0.0000f, 0.0000f, 0.0000f, 1.0000f) },
        { new Vector3(90, 0, 0), new Quaternion(0.7071f, 0.0000f, 0.0000f, 0.7071f) },
        { new Vector3(45, 0, 0), new Quaternion(0.3826f, 0.0000f, 0.0000f, 0.9238f) },
        { new Vector3(30, 15, 20), new Quaternion(0.2745f, 0.0796f, 0.1330f, 0.9489f) },
        { new Vector3(67, 48, 21), new Quaternion(0.5575f, 0.2416f, -0.0819f, 0.7899f) },
        { new Vector3(0, 90, 0), new Quaternion(0.0000f, 0.7071f, 0.0000f, 0.7071f) },
        { new Vector3(0, 0, 90), new Quaternion(0.0000f, 0.0000f, 0.7071f, 0.7071f) },
        { new Vector3(180, 0, 0), new Quaternion(1.0000f, 0.0000f, 0.0000f, 0.0000f) },
        { new Vector3(0, 180, 0), new Quaternion(0.0000f, 1.0000f, 0.0000f, 0.0000f) },
        { new Vector3(0, 0, 180), new Quaternion(0.0000f, 0.0000f, 1.0000f, 0.0000f) },
    };

    /// Calculated with https://www.andre-gaschler.com/rotationconverter/ -> using XYZ (Yaw, Pitch, Roll)
    /// Angles and their negative counterpart (e.g -90 CW and 270 CW)
    public static TheoryData<Vector3, Quaternion> TestCaseWithAnglesAndThereNegativeCounterpart => new()
    {
        { new Vector3(90, 0, 0), new Quaternion(0.7071f, 0.0000f, 0.0000f, 0.7071f) },
        { new Vector3(-270, 0, 0), new Quaternion(-0.7071f, 0.0000f, 0.0000f, -0.7071f) },
        { new Vector3(-90, 0, 0), new Quaternion(-0.7071f, 0.0000f, 0.0000f, 0.7071f) },
        { new Vector3(270, 0, 0), new Quaternion(0.7071f, 0.0000f, 0.0000f, -0.7071f) },
        { new Vector3(0, -60, 0), new Quaternion(0.0000f, -0.5000f, 0.0000f, 0.8660f) },
        { new Vector3(0, 300, 0), new Quaternion(0.0000f, 0.5000f, 0.0000f, -0.8660f) },
        { new Vector3(0, -300, 0), new Quaternion(0.0000f, -0.5000f, 0.0000f, -0.8660f) },
        { new Vector3(0, 60, 0), new Quaternion(0.0000f, 0.5000f, 0.0000f, 0.8660f) },
        { new Vector3(0, 0, 180), new Quaternion(0.0000f, 0.0000f, 1.0000f, 0.0000f) },
        { new Vector3(0, 0, -180), new Quaternion(0.0000f, 0.0000f, -1.000f, 0.0000f) },
        { new Vector3(0, 0, 45), new Quaternion(0.0000f, 0.0000f, 0.3826f, 0.9238f) },
        { new Vector3(0, 0, -315), new Quaternion(0.0000f, 0.0000f, -0.3826f, -0.9238f) },
    };

    [Theory]
    // [MemberData(nameof(TestCasesAndreGaschler))]
    // [MemberData(nameof(TestCaseWithAnglesAndThereNegativeCounterpart))]
    [MemberData(nameof(TestCasesWolframAlpha))]
    // [MemberData(nameof(TestCasesNormalConversion))]
    // [MemberData(nameof(TestCasesGimbalLock))]
    public void TestQuaternionConversion(Vector3 input, Quaternion expected)
    {
        const float precision = 0.0001f;

        var vectorRadian = new Vector3(
            input.X.DegreesToRadians(), 
            input.Y.DegreesToRadians(),
            input.Z.DegreesToRadians());
        var result = ConvertToQuaternion(vectorRadian);

        using var _ = new AssertionScope();
        result.X.Should().BeApproximately(expected.X, precision);
        result.Y.Should().BeApproximately(expected.Y, precision);
        result.Z.Should().BeApproximately(expected.Z, precision);
        result.W.Should().BeApproximately(expected.W, precision);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(90, 90)]
    [InlineData(180, 180)]
    [InlineData(359.9999, 359.9999)]
    [InlineData(360, 0)]
    [InlineData(-1, 359)]
    [InlineData(-90, 270)]
    [InlineData(-359.9999, 0.0001)]
    public void TestNormalizationOfAngle(float angle, float expected)
    {
        var result = angle.NormalizeAngle();
        result.Should().BeApproximately(expected, 0.00001f);
    }

    [Fact]
    public void TestEulerAngleToRotationMatrixViaQuaternion()
    {
        var vector = new Vector3(90, 0, 0);
        var expected = Matrix4x4.Identity;
        
        var vectorRadian = new Vector3(
            vector.X.DegreesToRadians(), 
            vector.Y.DegreesToRadians(),
            vector.Z.DegreesToRadians());
        var quaternion = ConvertToQuaternion(vectorRadian);
        var rotationMatrix = Matrix4x4.CreateFromQuaternion(quaternion);
        var eulerVectorRadian = ConvertFromRotationMatrixToEuler(rotationMatrix);

        eulerVectorRadian.Should().BeEquivalentTo(vectorRadian);
    }

    private Vector3 ConvertFromRotationMatrixToEuler(Matrix4x4 rotationMatrix)
    {
        var x = MathF.Asin(- float.Clamp(rotationMatrix.M23, -1.0f, 1.0f));
        var isNotGimbalLocked = MathF.Abs(rotationMatrix.M23) < 0.9999999f;

        var y = isNotGimbalLocked
            ? MathF.Atan2(rotationMatrix.M13, rotationMatrix.M33)
            : MathF.Atan2(rotationMatrix.M31 * -1, rotationMatrix.M11);
        
        var z = isNotGimbalLocked
            ? MathF.Atan2(rotationMatrix.M21, rotationMatrix.M22)
            : 0;

        return new Vector3(x, y, z);
    }

    private Quaternion ConvertToQuaternion(Vector3 vectorRadian)
    {
        // var normalizedVector = Vector3.Normalize(vector);
        // var angle = DegreesToRadians(vector.Length());

        // return Quaternion.CreateFromAxisAngle(normalizedVector, angle);
        
        // In comparison to Tree.Js library, the factor YawPitchRoll seems to use the YXZ (2-1-3) euler angles
        // for the conversion. See https://github.com/mrdoob/three.js/blob/master/src/math/Quaternion.js#L201
        var yaw = vectorRadian.Y;
        var pitch = vectorRadian.X;
        var roll = vectorRadian.Z;
        return Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);
    }
}

file static class Extension
{
    public static float DegreesToRadians(this float degrees)
    {
        return degrees * MathF.PI / 180.0f;
    }

    public static float NormalizeAngle(this float angle)
    {
        return (float)(((decimal)angle % 360m + 360m) % 360m);
    }
}
