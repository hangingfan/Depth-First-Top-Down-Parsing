# Depth-First-Top-Down-Parsing
corresponding link is the &lt;&lt;Parsing Techniques 2nd>>, Page 176

------------------------sampleOutput------------------------  
input your input string:  
aabc  
replace non-terminal in prediction, rule Index: 1  
                  aabc#  
            S1#   AB#  
replace non-terminal in prediction, rule Index: 1  
                  aabc#  
          A1S1#   aB#  
terminal matches, moveforward, char: a  
              a   abc#  
         aA1S1#   B#  
replace non-terminal in prediction, rule Index: 1  
              a   abc#  
       B1aA1S1#   bc#  
B --change to next match, index:2  
              a   abc#  
       B2aA1S1#   bBc#  
backtracking, non_terminal'rule is traversed: B  
              a   abc#  
         aA1S1#   B#  
backtracking, char: a  
                  aabc#  
          A1S1#   aB#  
A --change to next match, index:2  
                  aabc#  
          A2S1#   aAB#  
terminal matches, moveforward, char: a  
              a   abc#  
         aA2S1#   AB#  
replace non-terminal in prediction, rule Index: 1  
              a   abc#  
       A1aA2S1#   aB#  
terminal matches, moveforward, char: a  
             aa   bc#  
      aA1aA2S1#   B#  
replace non-terminal in prediction, rule Index: 1  
             aa   bc#  
    B1aA1aA2S1#   bc#  
terminal matches, moveforward, char: b  
            baa   c#  
   bB1aA1aA2S1#   c#  
terminal matches, moveforward, char: c  
           cbaa   #  
  cbB1aA1aA2S1#   #  
finished, the analysis is: cbB1aA1aA2S1#  
