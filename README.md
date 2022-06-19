# GTBuilderGraph

This was an effort to build a node-based procedural modelling tool on my GTLogicGraph repo here: https://github.com/rygo6/GTLogicGraph

It has enough nodes in it to make a procedural race track, it can extrude and loft curves, then refine their tesselation, and chop the mesh up into MeshRenderers/MeshCollider. All procuedurally, that automatically updates when you edit the root spline.

It works, but is not entirely done, some major quirks still. I've also generally I've abondoned this for now, and it probably won't work at all in newer Unity versions as too much changed in the UIElements Graph library. I may come back to this idea at some point but implement it to be based entirely on Burst and Job system, this one is change plain C# objects chaining together via C# events.
