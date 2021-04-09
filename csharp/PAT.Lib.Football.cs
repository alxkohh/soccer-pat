// namespace must be PAT.Lib

using System;

namespace PAT.Lib {
	
    /*
        Syntax to invoke ball_move_forward(...) in PAT:
        
        call(ball_move_forward, teamA, 2, ball_loc)
        
        call is a keyword in PAT
        
        Also, all methods need to be public static
    */
	
    public class Football {
	     
        private static int teamA = 0;
        private static int zones = 4;
        private static int shortPassDistance = 1;
        private static int longPassDistance = 2;
        
        private static double shoot_k = 3.049;
        private static double shoot_x0 = 1.766;
        private static double s_pass_k = 2.024;
        private static double s_pass_x0 = 0.033;
        private static double l_pass_k = 3.047;
        private static double l_pass_x0 = 0.888;
        private static double dribble_k = 1.417;
        private static double dribble_x0 = 0.730;

        public static void Config(int _zones, int _shortPassDistance, int _longPassDistance)
        {
            zones = _zones;
            shortPassDistance = _shortPassDistance;
            longPassDistance = _longPassDistance;
        }
        
        public static void setZones(int _zones) {
            zones = _zones;
        }

        public static int opponent_of(int team) {
            return 1 - team;
        }

        private static double L(double x, double k, double x0)
        {
            return 100 / (1 + Math.Exp(-k * (x - x0)));
        }
        
        public static int get_zones() {
        	return zones;
        }
        
        public static int get_loc(int ball_loc) {
        	return (ball_loc * 100)/zones;
        }
        
        public static int can_shoot(int team, int ball_loc) {
        	if (team == teamA)
        		return  (ball_loc >= 7) ? 1 : 0;
 			return (ball_loc < 3) ? 1 : 0;
        }

        public static int shoot_success_rate(int def_team, int ball_loc, int scoreA, int scoreB)
        {
            var ratio = (double) scoreA / scoreB;
            return (int) L(ratio, shoot_k, shoot_x0) * can_shoot(1-def_team, ball_loc);
        }
        
        public static int short_pass_success_rate(int team, int ball_loc, int scoreA, int scoreB)
        {
            var ratio = (double) scoreA / scoreB;
            return (int) L(ratio, s_pass_k, s_pass_x0);
        }
        
        public static int long_pass_success_rate(int team, int ball_loc, int scoreA, int scoreB)
        {
            var ratio = (double) scoreA / scoreB;
            return (int) L(ratio, l_pass_k, l_pass_x0);
        }
        
        public static int dribble_success_rate(int team, int ball_loc, int scoreA, int scoreB)
        {
            var ratio = (double) scoreA / scoreB;
            return (int) L(ratio, dribble_k, dribble_x0);
        }

        public static int success_rate(int team, int ball_loc) {
            if (team == teamA) {
                return (zones - ball_loc) * 100/zones;
            } else {
                return ball_loc * 100/zones;
            }
        }
	    
        // ball_move_forward(teamA, 2, ball_loc) means the ball move forward 2 zones for teamA. new ball location is returned
        public static int ball_move_forward(int team, int numZones, int loc) {
            if (team == teamA) {
                return ball_move_forward_teamA(numZones, loc);
            } else {	// team == teamB
                return ball_move_forward_teamB(numZones, loc);
            }
        }
        private static int ball_move_forward_teamA(int numZones, int loc) {
            int newLoc = loc + numZones;
            // the furthest teamA can go is zones - 1
            if (newLoc > zones - 1) {
                return zones - 1;
            } else {
                return newLoc;
            }
        }

        private static int ball_move_forward_teamB(int numZones, int loc) {
			int newLoc = loc - numZones;
            // the furthest teamB can go is 0
            if (newLoc < 0) {
                return 0;
            } else {
                return newLoc;
            }
        } 
	     
    }
}