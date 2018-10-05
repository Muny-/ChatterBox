using System;
using Cairo;
using Gdk;
using Gtk;
using System.Linq;

namespace ChatterBox
{
    public class CellRendererWidget : CellRenderer
    {

        //property asigned to by treeviewcolumn
        //eg. TreeViewColumn tvc = new TreeViewColumn("Column 0", new CellRendererSimple(),"tree_item",0);
        [GLib.Property("widget")] //this line seems to be nessasary for the property to be recognised by the treeviewcolumn.
        public Widget widget
        {
            get
            {
                return _widget;
            }
            set {
                Console.WriteLine($"ofsw == null: [{ofsw == null}]");
                //value.Toplevel = ofsw;
                _widget = value;
            }
        }

        private Widget _widget;

        Gtk.OffscreenWindow ofsw = new Gtk.OffscreenWindow();

		protected override void OnRender(Context cr, Widget __other_widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, CellRendererState flags)
		{
            base.OnRender(cr, __other_widget, background_area, cell_area, flags);

            //Cairo.ImageSurface surf = new Cairo.ImageSurface(Cairo.Format.Argb32, expose_area.Width, expose_area.Height);




            widget.Visible = true;

            //widget.SizeAllocate(cell_area);
            //widget.SetAllocation(cell_area);
            widget.SetAllocation(cell_area);
            widget.Draw(cr);
		}

        /*protected override void Render(Drawable window, Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, Gdk.Rectangle expose_area, CellRendererState flags)
        {
            //this seems to be the minimum to render text
            GC gc = widget.Style.TextGC(StateType.Normal);
            Pango.Layout layout = new Pango.Layout(widget.PangoContext);
            layout.SetText(tree_item);//add the text to thelayout.
                                      //this is the actuall rendering
            window.DrawLayout(gc, expose_area.X, expose_area.Y, layout);
        }*/

        /*
         * 
         * 
         * > main()
            > {
            >     GtkDrawingArea *statistics;
            >     cairo_surface_t *surface;
            >     cairo_t *cr;
            >     GtkWindow *main_window;
            >
            >
            >     gtk_init( NULL, NULL );
            >     statistics = (GtkDrawingArea*)gtk_drawing_area_new();
            >     surface = cairo_image_surface_create (CAIRO_FORMAT_ARGB32, 120, 120);
            >     cr = cairo_create ( surface );
            >     gtk_widget_draw( (GtkWidget*)statistics, cr );
            >     cairo_surface_write_to_png( surface, "one.png" );
            >     main_window = (GtkWindow*)gtk_window_new(GTK_WINDOW_TOPLEVEL);
            >
            > g_signal_connect(G_OBJECT(main_window),"destroy",G_CALLBACK(gtk_main_quit),NULL);
            >     gtk_container_add (GTK_CONTAINER (main_window),(GtkWidget*)statistics);
            >     gtk_widget_show_all((GtkWidget*)main_window);
            >
            >     gtk_main();
            > }
         * 
         * main()
            {
                GtkDrawingArea *statistics;
                cairo_surface_t *surface;
                cairo_t *cr;
                GtkWindow *main_window;


                gtk_init( NULL, NULL );
                statistics = (GtkDrawingArea*)gtk_drawing_area_new();
                surface = cairo_image_surface_create (CAIRO_FORMAT_ARGB32, 120, 120);
                cr = cairo_create ( surface );
                g_signal_connect(G_OBJECT(statistics), "draw",
            G_CALLBACK(on_draw_event), NULL);
                main_window = (GtkWindow*)gtk_window_new(GTK_WINDOW_TOPLEVEL);

            g_signal_connect(G_OBJECT(main_window),"destroy",G_CALLBACK(gtk_main_quit),NULL);
                gtk_container_add (GTK_CONTAINER (main_window),(GtkWidget*)statistics);
                gtk_widget_show_all((GtkWidget*)main_window);

                puts(gtk_widget_is_drawable((GtkWidget*)statistics) ? "yes" : "no");
                gtk_widget_draw( (GtkWidget*)statistics, cr );
                cairo_surface_write_to_png( surface, "one.png" );

                gtk_main();
            }
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * */
    }
}
