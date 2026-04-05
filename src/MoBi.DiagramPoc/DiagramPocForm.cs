using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Diagram.Core;
using DevExpress.Utils;
using DevExpress.XtraDiagram;

namespace MoBi.DiagramPoc
{
   public class DiagramPocForm : Form
   {
      private TabControl _tabs;

      public DiagramPocForm()
      {
         Text = "MoBi Diagram POC — DevExpress DiagramControl";
         Size = new Size(1200, 800);
         StartPosition = FormStartPosition.CenterScreen;

         _tabs = new TabControl { Dock = DockStyle.Fill };
         Controls.Add(_tabs);

         addReactionTab();
         addContainerTab();
         addLayoutTab();
      }

      // ─────────────────────────────────────────────────────
      // POC 1: Reaction network with custom shapes and ports
      // ─────────────────────────────────────────────────────
      private void addReactionTab()
      {
         var tab = new TabPage("POC 1: Reaction Network");
         var diagram = createDiagram();

         // Molecule nodes (circles with 1 connection point)
         var molA = createMoleculeNode("A", 50, 200, Color.CornflowerBlue);
         var molB = createMoleculeNode("B", 500, 100, Color.MediumSeaGreen);
         var molC = createMoleculeNode("C", 500, 300, Color.IndianRed);

         // Reaction node (triangle-like with 3 ports: educt=blue, product=green, modifier=red)
         var reaction = createReactionNode("R1", 270, 180);

         diagram.Items.Add(molA);
         diagram.Items.Add(molB);
         diagram.Items.Add(molC);
         diagram.Items.Add(reaction);

         // Connectors: A --educt--> R1, R1 --product--> B, C --modifier--> R1
         var eductLink = createConnector(molA, reaction, 0, 0, Color.Blue, "educt");
         var productLink = createConnector(reaction, molB, 1, 0, Color.Green, "product");
         var modifierLink = createConnector(molC, reaction, 0, 2, Color.Red, "modifier");

         diagram.Items.Add(eductLink);
         diagram.Items.Add(productLink);
         diagram.Items.Add(modifierLink);

         // Log connection changes to verify port-specific detection
         diagram.ConnectionChanged += (s, e) =>
         {
            var connector = e.Connector as DiagramConnector;
            if (connector == null) return;
            var beginName = (connector.BeginItem as DiagramShape)?.Content ?? "?";
            var endName = (connector.EndItem as DiagramShape)?.Content ?? "?";
            var msg = $"Connection changed: BeginItem={beginName}, " +
                      $"BeginPointIndex={connector.BeginItemPointIndex}, " +
                      $"EndItem={endName}, " +
                      $"EndPointIndex={connector.EndItemPointIndex}";
            System.Diagnostics.Debug.WriteLine(msg);
         };

         var panel = new Panel { Dock = DockStyle.Top, Height = 40 };
         var label = new Label
         {
            Text = "Drag from molecule to reaction ports. Blue=educt(left), Green=product(right), Red=modifier(bottom).",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(8)
         };
         panel.Controls.Add(label);

         tab.Controls.Add(diagram);
         tab.Controls.Add(panel);
         _tabs.TabPages.Add(tab);
      }

      // ─────────────────────────────────────────────────────
      // POC 2: Nested containers with expand/collapse
      // ─────────────────────────────────────────────────────
      private void addContainerTab()
      {
         var tab = new TabPage("POC 2: Nested Containers");
         var diagram = createDiagram();

         // Read-only mode: allow move and zoom but not editing
         diagram.OptionsProtection.IsReadOnly = true;
         diagram.OptionsProtection.AllowMoveItems = true;
         diagram.OptionsProtection.AllowZoom = true;
         diagram.OptionsProtection.AllowCollapseContainers = true;

         // Organism > Organ > Compartment hierarchy
         var compartment1 = createContainer("Plasma", 20, 20, 120, 60, Color.LightCyan);
         var compartment2 = createContainer("Interstitial", 20, 100, 120, 60, Color.LightCyan);

         var liver = createContainer("Liver", 10, 10, 180, 200, Color.LightGoldenrodYellow);
         liver.Items.Add(compartment1);
         liver.Items.Add(compartment2);

         var compartment3 = createContainer("Plasma", 20, 20, 120, 60, Color.LightCyan);
         var kidney = createContainer("Kidney", 10, 10, 180, 120, Color.LightGoldenrodYellow);
         kidney.Items.Add(compartment3);

         var organism = createContainer("Organism", 20, 20, 450, 350, Color.Lavender);
         organism.Items.Add(liver);
         organism.Items.Add(kidney);

         diagram.Items.Add(organism);

         // Neighborhood link between Liver.Plasma and Kidney.Plasma
         var neighborLink = new DiagramConnector();
         neighborLink.BeginItem = compartment1;
         neighborLink.EndItem = compartment3;
         neighborLink.Appearance.BorderColor = Color.DarkOrange;
         neighborLink.Content = "Neighborhood";
         diagram.Items.Add(neighborLink);

         var panel = new Panel { Dock = DockStyle.Top, Height = 40 };
         var label = new Label
         {
            Text = "Read-only: expand/collapse containers, drag to reposition, zoom with Ctrl+wheel. No editing.",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(8)
         };
         panel.Controls.Add(label);

         tab.Controls.Add(diagram);
         tab.Controls.Add(panel);
         _tabs.TabPages.Add(tab);
      }

      // ─────────────────────────────────────────────────────
      // POC 3: Sugiyama auto-layout
      // ─────────────────────────────────────────────────────
      private void addLayoutTab()
      {
         var tab = new TabPage("POC 3: Auto-Layout");
         var diagram = createDiagram();

         // Build a reaction network: 5 molecules, 3 reactions, randomly positioned
         var rng = new Random(42);
         var mols = new[] { "Drug", "Metabolite1", "Metabolite2", "Enzyme", "Product" };
         var molNodes = new DiagramShape[mols.Length];
         for (int i = 0; i < mols.Length; i++)
         {
            molNodes[i] = createMoleculeNode(mols[i], rng.Next(50, 700), rng.Next(50, 500), Color.CornflowerBlue);
            diagram.Items.Add(molNodes[i]);
         }

         var rxns = new[] { "Metabolism", "Transport", "Elimination" };
         var rxnNodes = new DiagramShape[rxns.Length];
         for (int i = 0; i < rxns.Length; i++)
         {
            rxnNodes[i] = createReactionNode(rxns[i], rng.Next(50, 700), rng.Next(50, 500));
            diagram.Items.Add(rxnNodes[i]);
         }

         // Wire them up: Drug->Metabolism->Metabolite1, Drug->Transport->Metabolite2, Metabolite1->Elimination->Product
         addConnector(diagram, molNodes[0], rxnNodes[0], Color.Blue);    // Drug -> Metabolism (educt)
         addConnector(diagram, rxnNodes[0], molNodes[1], Color.Green);   // Metabolism -> Metabolite1 (product)
         addConnector(diagram, molNodes[3], rxnNodes[0], Color.Red);     // Enzyme -> Metabolism (modifier)
         addConnector(diagram, molNodes[0], rxnNodes[1], Color.Blue);    // Drug -> Transport (educt)
         addConnector(diagram, rxnNodes[1], molNodes[2], Color.Green);   // Transport -> Metabolite2 (product)
         addConnector(diagram, molNodes[1], rxnNodes[2], Color.Blue);    // Metabolite1 -> Elimination (educt)
         addConnector(diagram, rxnNodes[2], molNodes[4], Color.Green);   // Elimination -> Product

         var panel = new Panel { Dock = DockStyle.Top, Height = 40 };
         var btnLayout = new Button
         {
            Text = "Apply Sugiyama Layout",
            Dock = DockStyle.Left,
            Width = 200,
            Height = 36
         };
         btnLayout.Click += (s, e) =>
         {
            diagram.ApplySugiyamaLayout(diagram.Items.ToArray());
            diagram.FitToDrawing();
         };

         var btnTree = new Button
         {
            Text = "Apply Tree Layout",
            Dock = DockStyle.Left,
            Width = 200,
            Height = 36
         };
         btnTree.Click += (s, e) =>
         {
            diagram.ApplyTreeLayout(diagram.Items.ToArray());
            diagram.FitToDrawing();
         };

         panel.Controls.Add(btnTree);
         panel.Controls.Add(btnLayout);

         tab.Controls.Add(diagram);
         tab.Controls.Add(panel);
         _tabs.TabPages.Add(tab);
      }

      // ─────────────────────────────────────────────────────
      // Helpers
      // ─────────────────────────────────────────────────────
      private DiagramControl createDiagram()
      {
         var diagram = new DiagramControl { Dock = DockStyle.Fill };
         return diagram;
      }

      private DiagramShape createMoleculeNode(string name, float x, float y, Color color)
      {
         var shape = new DiagramShape
         {
            Shape = BasicShapes.Ellipse,
            Width = 70,
            Height = 70,
            Content = name,
            Position = new PointFloat(x, y),
         };
         shape.Appearance.BackColor = color;
         shape.Appearance.BorderColor = Color.FromArgb(60, 60, 60);

         // Connection points at cardinal positions for molecules
         shape.ConnectionPoints = new PointCollection(new[]
         {
            new PointFloat(0.5f, 0f),    // top
            new PointFloat(1f, 0.5f),    // right
            new PointFloat(0.5f, 1f),    // bottom
            new PointFloat(0f, 0.5f)     // left
         });

         return shape;
      }

      private DiagramShape createReactionNode(string name, float x, float y)
      {
         var shape = new DiagramShape
         {
            Shape = BasicShapes.Triangle,
            Width = 80,
            Height = 70,
            Content = name,
            Position = new PointFloat(x, y),
         };
         shape.Appearance.BackColor = Color.LightGray;
         shape.Appearance.BorderColor = Color.FromArgb(60, 60, 60);

         // 3 connection points: left=educt(blue), right=product(green), bottom=modifier(red)
         shape.ConnectionPoints = new PointCollection(new[]
         {
            new PointFloat(0f, 0.5f),    // index 0: educt (left)
            new PointFloat(1f, 0.5f),    // index 1: product (right)
            new PointFloat(0.5f, 1f)     // index 2: modifier (bottom)
         });

         return shape;
      }

      private DiagramContainer createContainer(string name, float x, float y, float w, float h, Color color)
      {
         var container = new DiagramContainer
         {
            Header = name,
            Position = new PointFloat(x, y),
            Width = w,
            Height = h,
            CanCollapse = true,
         };
         container.Appearance.BackColor = color;
         container.Appearance.BorderColor = Color.FromArgb(120, 120, 120);
         return container;
      }

      private DiagramConnector createConnector(DiagramItem from, DiagramItem to,
         int fromPointIndex, int toPointIndex, Color color, string label)
      {
         var connector = new DiagramConnector(from, to);
         connector.BeginItemPointIndex = fromPointIndex;
         connector.EndItemPointIndex = toPointIndex;
         connector.Appearance.BorderColor = color;
         connector.Content = label;
         return connector;
      }

      private void addConnector(DiagramControl diagram, DiagramItem from, DiagramItem to, Color color)
      {
         var connector = new DiagramConnector(from, to);
         connector.Appearance.BorderColor = color;
         diagram.Items.Add(connector);
      }
   }
}
