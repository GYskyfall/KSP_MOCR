﻿using KRPC.Client.Services.KRPC;
using KRPC.Client.Services.SpaceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSP_MOCR
{
	class OrbitView : MocrScreen
	{
		CelestialBody body;
		float bodyRadius;
		String bodyName;

		// TELEMETRY FIELDS
		double apopapsis;
		double periapsis;
		double eccentricity;
		double inclination;
		double sMa;
		double sma;
		double argOP;
		double lOAN;
		double radius;
		double trueAnomaly;
		double timeToPe;
		double timeToAp;
		double period;

		IList<Node> nodes;
		ReferenceFrame inerFrame;

		public OrbitView(Screen form)
		{
			this.form = form;
			this.screenStreams = form.streamCollection;

			this.charSize = false;
			this.width = 674;
			this.height = 508;
			this.updateRate = 1000;

			
		}

		public override void makeElements()
		{
			for (int i = 0; i < 1; i++) screenInputs.Add(null); // Initialize Inputs
			screenInputs[0] = Helper.CreateInput(-2, -2, 1, 2); // Every page must have an input to capture keypresses on Unix

			screenOrbit = Helper.CreateOrbit(0, 0, 674, 508, true);	
		}

		public override void updateLocalElements(object sender, EventArgs e)
		{
			if (form.form.connected && form.form.krpc.CurrentGameScene == GameScene.Flight)
			{
				period = screenStreams.GetData(DataType.orbit_period);
				apopapsis = screenStreams.GetData(DataType.orbit_apoapsis);
				periapsis = screenStreams.GetData(DataType.orbit_periapsis);
				sMa = screenStreams.GetData(DataType.orbit_semiMajorAxis);
				sma = screenStreams.GetData(DataType.orbit_semiMinorAxis);
				argOP = screenStreams.GetData(DataType.orbit_argumentOfPeriapsis);
				lOAN = screenStreams.GetData(DataType.orbit_longitudeOfAscendingNode);
				eccentricity = screenStreams.GetData(DataType.orbit_eccentricity);
				inclination = screenStreams.GetData(DataType.orbit_inclination);
				radius = screenStreams.GetData(DataType.orbit_radius);
				trueAnomaly = screenStreams.GetData(DataType.orbit_trueAnomaly);

				nodes = screenStreams.GetData(DataType.control_nodes);

				body = screenStreams.GetData(DataType.orbit_celestialBody);
				if (body != null)
				{
					bodyRadius = body.EquatorialRadius;
					bodyName = body.Name;

					IList<CelestialBody> bodySatellites = body.Satellites;
					screenOrbit.setBody(body, bodyRadius, bodyName, bodySatellites);
					screenOrbit.setOrbit(apopapsis, periapsis, sMa, sma, argOP, lOAN, radius, trueAnomaly, inclination);

					if (nodes != null && nodes.Count > 0)
					{
						inerFrame = body.NonRotatingReferenceFrame;
						Node node = nodes[0];
						Tuple<double, double, double> burnPos = node.Position(inerFrame);
						Tuple<double, double, double> burnVel = node.BurnVector(inerFrame);
						screenOrbit.setBurnData(burnVel, burnPos);
					}
				}
				screenOrbit.Invalidate();
			}
		}

		public override void resize()
		{
			if (screenOrbit != null)
			{
				screenOrbit.Size = form.ClientSize;
			}
		}

		
	}
}
