using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace TestXamarinAndroid
{
    [Activity(Label = "Fill Medispencer")]
    public class FillMedispencer : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.FillMedispencer);

            FillInstructions();

            Button filledButton = FindViewById<Button>(Resource.Id.filledButton);
            filledButton.Click += (object sender, EventArgs e) =>
            {
                // TODO: move to main activity and send 'filled' to the server
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };
        }

        private void FillInstructions()
        {
            for (var i = 0; i < MedispencerData.s_cells.Length/*m_cellFillInstructions.Length*/; i++)
            {
                FillCellInstruction(i);
            }
        }

        private void FillCellInstruction(int i)
        {
            // fill cell i
            if (MedispencerData.s_cells[i].m_pillType.Length > 0)
            {
                TextView titleTextView = GetCellTitleTextView(i);
                titleTextView.Text = "Cell " + (i + 1) + ":";
                // textViewCell1Desc
                TextView descTextView = GetCellDescTextView(i);
                descTextView.Text = GetCellDescription(MedispencerData.s_cells[i]);
            }
        }

        private TextView GetCellTitleTextView(int i)
        {
            switch (i)
            {
                case 0:
                    return FindViewById<TextView>(Resource.Id.textViewCell1);
                case 1:
                    return FindViewById<TextView>(Resource.Id.textViewCell2);
                case 2:
                    return FindViewById<TextView>(Resource.Id.textViewCell3);
                case 3:
                    return FindViewById<TextView>(Resource.Id.textViewCell4);
                case 4:
                    return FindViewById<TextView>(Resource.Id.textViewCell5);
                default:
                    return null;
            }
        }

        private TextView GetCellDescTextView(int i)
        {
            switch (i)
            {
                case 0:
                    return FindViewById<TextView>(Resource.Id.textViewCell1Desc);
                case 1:
                    return FindViewById<TextView>(Resource.Id.textViewCell2Desc);
                case 2:
                    return FindViewById<TextView>(Resource.Id.textViewCell3Desc);
                case 3:
                    return FindViewById<TextView>(Resource.Id.textViewCell4Desc);
                case 4:
                    return FindViewById<TextView>(Resource.Id.textViewCell5Desc);
                default:
                    return null;
            }
        }

        private string GetCellDescription(Cell cellFillInstruction)
        {
            return "Add " + cellFillInstruction.m_numOfPills + " pill of " + cellFillInstruction.m_pillType;
        }
    }

}