using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Umsetzung_III.Actions;

namespace Umsetzung_III
{
    public class ScoreCommand : CommandBase
    {
        ViewModel spielanzeige;
        Team team;
        StandVeraenderung standVeraenderung;

        public ScoreCommand(ViewModel spielanzeige, Team team, StandVeraenderung standVeraenderung)
        {
            this.spielanzeige = spielanzeige;
            this.team = team;
            this.standVeraenderung = standVeraenderung;

        }
        public override void Execute(object? parameter)
        {
            switch (this.team)
            {
                case Team.Gast:
                    switch (this.standVeraenderung)
                    {
                        case StandVeraenderung.Hoch:
                            spielanzeige.GastTeamScore++;
                            break;
                        case StandVeraenderung.Runter:
                            if(spielanzeige.GastTeamScore > 0)
                            {
                                spielanzeige.GastTeamScore--;
                            }
                            break;
                    }
                    spielanzeige.OnPropertyChanged("GastTeamScore");
                    break;

                case Team.Heim:
                    switch (this.standVeraenderung)
                    {
                        case StandVeraenderung.Hoch:
                            spielanzeige.HeimTeamScore++;
                            break;
                        case StandVeraenderung.Runter:
                            if(spielanzeige.HeimTeamScore > 0)
                            {
                                spielanzeige.HeimTeamScore--;
                            }
                            break;
                    }
                    spielanzeige.OnPropertyChanged("HeimTeamScore");
                    break;
            }
        }
    }
}
