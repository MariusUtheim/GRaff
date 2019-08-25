using System;
using GRaff.Randomness;

namespace GRaff.Graphics.Particles
{
    public class InitialStateDescriptor : IParticleTypeDescriptor
    {
        public InitialStateDescriptor()
        { }

        public InitialStateDescriptor(IDistribution<Vector> scale, IDistribution<Angle> rotation)
        {
            ScaleDistribution = scale;
            RotationDistribution = rotation;
        }

        public InitialStateDescriptor ConstantRotation(Angle angle)
        {
            RotationDistribution = new ConstantDistribution<Angle>(angle);
            return this;
        }
        public InitialStateDescriptor UniformRotation()
        {
            RotationDistribution = new AngleDistribution();
            return this;
        }
        public InitialStateDescriptor UniformRotation(Angle min, Angle max)
        {
            RotationDistribution = new AngleDistribution(min, max);
            return this;
        }
        public InitialStateDescriptor Rotation(IDistribution<Angle> distribution)
        {
            RotationDistribution = distribution;
            return this;
        }

        public InitialStateDescriptor ConstantScale(double scale)
        {
            ScaleDistribution = new ConstantDistribution<Vector>((scale, scale));
            return this;
        }
        public InitialStateDescriptor UniformScale(double min, double max)
        {
            ScaleDistribution = new FuncDistribution<Vector>(() => { var s = GRandom.Double(min, max); return (s, s); });
            return this;
        }
        public InitialStateDescriptor Scale(IDistribution<double> distribution)
        {
            ScaleDistribution = distribution.Transform(d => new Vector(d, d));
            return this;
        }

        public InitialStateDescriptor ConstantScaleXY(double xScale, double yScale)
        {
            ScaleDistribution = new ConstantDistribution<Vector>((xScale, yScale));
            return this;
        }
        public InitialStateDescriptor UniformScaleXY(double xScale, (double min, double max) yScale)
        {
            ScaleDistribution = new CartesianVectorDistribution(xScale, yScale);
            return this;
        }
        public InitialStateDescriptor UniformScaleXY((double min, double max) xScale, double yScale)
        {
            ScaleDistribution = new CartesianVectorDistribution(xScale, yScale);
            return this;
        }
        public InitialStateDescriptor UniformScaleXY((double min, double max) xScale, (double min, double max) yScale)
        {
            ScaleDistribution = new CartesianVectorDistribution(xScale, yScale);
            return this;
        }
        public InitialStateDescriptor ScaleXY(IDistribution<double> xDistribution, IDistribution<double> yDistribution)
        {
            ScaleDistribution = new CartesianVectorDistribution(xDistribution, yDistribution);
            return this;
        }
        public InitialStateDescriptor ScaleXY(IDistribution<Vector> scaleDistribution)
        {
            ScaleDistribution = scaleDistribution;
            return this;
        }

        public IDistribution<Vector> ScaleDistribution { get; set; }

        public IDistribution<Angle> RotationDistribution { get; set; }

        class InitialStateBehavior : IParticleBehavior
        {
            public Matrix Transform;

            public void Initialize(Particle particle) => particle.TransformationMatrix = Transform;

            public void Update(Particle particle) { }
        }

        public IParticleBehavior MakeBehavior()
        {
            var transform = new Transform();
            if (ScaleDistribution != null)
                transform.Scale = ScaleDistribution.Generate();
            if (RotationDistribution != null)
                transform.Rotation = RotationDistribution.Generate();

            return new InitialStateBehavior { Transform = transform.GetMatrix() };
        }
    }
}
