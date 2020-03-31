using System;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;
using NUnit.Framework;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.Tests.Framework;

static class CalculateNormalsTests
{
    const float k_NormalCompareEpsilon = .0001f;

    [Test]
    public static void Calculate_HardNormals_AreNormalized()
    {
        using (var shapes = new TestUtility.BuiltInPrimitives())
        {
            foreach (var pb in (IEnumerable<ProBuilderMesh>)shapes)
            {
                foreach (var face in pb.faces)
                    face.smoothingGroup = Smoothing.smoothingGroupNone;

                pb.Refresh(RefreshMask.Normals);

                Vector3[] normals = pb.GetNormals();

                foreach (var nrm in normals)
                {
                    Assert.AreEqual(1f, nrm.magnitude, k_NormalCompareEpsilon, pb.name);
                }
            }
        }
    }

    [Test]
    public static void Calculate_SoftNormals_AreNormalized()
    {
        using (var shapes = new TestUtility.BuiltInPrimitives())
        {
            foreach (var pb in (IEnumerable<ProBuilderMesh>)shapes)
            {
                foreach (var face in pb.faces)
                    face.smoothingGroup = 1;

                pb.Refresh(RefreshMask.Normals);

                Vector3[] normals = pb.GetNormals();

                foreach (var nrm in normals)
                {
                    Assert.AreEqual(1f, nrm.magnitude, k_NormalCompareEpsilon, pb.name);
                }
            }
        }
    }

    [Test]
    public static void Calculate_SoftNormals_AreSoft()
    {
        using (var shapes = new TestUtility.BuiltInPrimitives())
        {
            foreach (var pb in (IEnumerable<ProBuilderMesh>)shapes)
            {
                foreach (var face in pb.faces)
                    face.smoothingGroup = 1;

                pb.ToMesh();
                pb.Refresh();

                Vector3[] normals = pb.GetNormals();

                foreach (var common in pb.sharedVertices)
                {
                    int[] arr = common.arrayInternal;
                    Vector3 nrm = normals[arr[0]];

                    for (int i = 1, c = arr.Length; i < c; i++)
                        Assert.AreEqual(nrm, normals[arr[i]]);
                }
            }
        }
    }
}
