using System;
using System.Collections.Generic;
using System.Text;

// namespace must be PAT.Lib
namespace PAT.Lib {
	
	/*
		Syntax to invoke ball_move_forward(...) in PAT:
		
		call(ball_move_forward, teamA, 2, ball_loc)
		
		call is a keyword in PAT
		
		Also, all methods need to be public static
	*/
	
    public class Football {
	     
	 	private static int teamA = -1;
	 	
	 	public static int opponent_of(int team) {
	 		return -1 * team;
	 	}
	 	
	 	public static int success_rate(int team, int ball_loc) {
			if (team == teamA) {
				return 100 - ball_loc;
			} else {
				return ball_loc;
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
	    
	    public static int ball_move_forward_teamA(int numZones, int loc) {
			
			if (numZones == 1) {
				
				int new_loc = loc + 20;
				
				// the furthest teamA can go is 80
				if (new_loc > 80) {
					return 80;
				} else {
					return new_loc;
				}
			} else {	// numZones == 2
				
				int new_loc = loc + 40;
				
				// the furthest teamA can go is 80
				if (new_loc > 80) {
					return 80;
				} else {
					return new_loc;
				}
			}
		}
		
		public static int ball_move_forward_teamB(int numZones, int loc) {
			
			if (numZones == 1) {
				
				int new_loc = loc - 20;
				
				// the furthest teamB can go is 20
				if (new_loc < 20) {
					return 20;
				} else {
					return new_loc;
				}
			} else {	// numZones == 2
				
				int new_loc = loc - 40;
				
				// the furthest teamB can go is 20
				if (new_loc < 20) {
					return 20;
				} else {
					return new_loc;
				}
			}
		} 
	     
    }
}
