using System;
using GRaff.Randomness;

namespace GRaff.Graphics.Particles
{
    public class TransformationDescriptor : IParticleTypeDescriptor
    {
        public TransformationDescriptor()
        { }

        public TransformationDescriptor(IDistribution<Vector> scale, IDistribution<Angle> rotation)
        {
            ScalingDistribution = scale;
            RotationDistribution = rotation;
        }

        public TransformationDescriptor ConstantRotation(Angle angle)
        {
            RotationDistribution = new ConstantDistribution<Angle>(angle);
            return this;
        }
        public TransformationDescriptor UniformRotation()
        {
            RotationDistribution = new AngleDistribution();
            return this;
        }
        public TransformationDescriptor UniformRotation(Angle min, Angle max)
        {
            RotationDistribution = new AngleDistribution(min, max);
            return this;
        }
        public TransformationDescriptor Rotation(IDistribution<Angle> distribution)
        {
            RotationDistribution = distribution;
            return this;
        }

        public TransformationDescriptor ConstantScaling(double scale)
        {
            ScalingDistribution = new ConstantDistribution<Vector>((scale, scale));
            return this;
        }
        public TransformationDescriptor UniformScaling(double min, double max)
        {
            ScalingDistribution = new FuncDistribution<Vector>(() => { var s = GRandom.Double(min, max); return (s, s); });
            return this;
        }
        public TransformationDescriptor Scaling(IDistribution<double> distribution)
        {
            ScalingDistribution = distribution.Transform(d => new Vector(d, d));
            return this;
        }

        public TransformationDescriptor ConstantScalingXY(double xScale, double yScale)
        {
            ScalingDistribution = new ConstantDistribution<Vector>((xScale, yScale));
            return this;
        }
        public TransformationDescriptor UniformScalingXY(double xScale, (double min, double max) yScale)
        {
            ScalingDistribution = new CartesianVectorDistribution(xScale, yScale);
            return this;
        }
        public TransformationDescriptor UniformScalingXY((double min, double max) xScale, double yScale)
        {
            ScalingDistribution = new CartesianVectorDistribution(xScale, yScale);
            return this;
        }
        public TransformationDescriptor UniformScalingXY((double min, double max) xScale, (double min, double max) yScale)
        {
            ScalingDistribution = new CartesianVectorDistribution(xScale, yScale);
            return this;
        }
        public TransformationDescriptor ScalingXY(IDistribution<double> xDistribution, IDistribution<double> yDistribution)
        {
            ScalingDistribution = new CartesianVectorDistribution(xDistribution, yDistribution);
            return this;
        }
        public TransformationDescriptor ScalingXY(IDistribution<Vector> scalingDistribution)
        {
            ScalingDistribution = ScalingDistribution;
            return this;
        }

        public IDistribution<Vector>? ScalingDistribution { get; set; }

        public IDistribution<Angle>? RotationDistribution { get; set; }


        class TransformationBehavior : IParticleBehavior
        {
            private Matrix _transform;
            public TransformationBehavior(Matrix transform)
            {
                this._transform = transform;
            }
            public void Initialize(Particle particle) { }

            public void Update(Particle particle) => particle.TransformationMatrix = _transform * particle.TransformationMatrix;
        }

        public IParticleBehavior MakeBehavior()
        {
            var transform = new Transform();
            if (ScalingDistribution != null)
                transform.Scale = ScalingDistribution.Generate();
            if (RotationDistribution != null)
                transform.Rotation = RotationDistribution.Generate();

            return new TransformationBehavior(transform.GetMatrix());
        }
    }
}
