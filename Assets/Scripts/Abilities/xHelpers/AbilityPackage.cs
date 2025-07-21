using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SoulCourier.Abilities;

// Basically a package that has data values preset, will be used using 'using' 
namespace SoulCourier.Abilities
{
    // Enum defines the different ability types to be selected in the scripts
    public enum AbilityType
    {
        Passive,
        Active,
        OneShot
    }

    public enum AbilityTier
    {
        S,
        A,
        B,
        C,
        D
    }

    public static class AbilityTierHelper
    {
        // Creates a dictionary of the ability tiers and adds weights which will determine how likley they are to spawn
        private static readonly Dictionary<AbilityTier, float> weights = new Dictionary<AbilityTier, float>
        {
            {  AbilityTier.S, 2f },
            { AbilityTier.A, 10f },
            { AbilityTier.B, 25f },
            { AbilityTier.C, 30f },
            { AbilityTier.D, 30f }
        };

        // Gets a random tier based on luck
        public static AbilityTier GetRandomTier(float luckMultiplier)
        {
            // Boosts rare abilities chance based on current luck
            var adjustedWeights = weights.ToDictionary(
                pair => pair.Key,
                pair => (pair.Key == AbilityTier.S || pair.Key == AbilityTier.A) // Boosts S and A tiers otherwise keeps weight the same
                    ? pair.Value * luckMultiplier
                    : pair.Value
            );

            // Generates a random weight by adding all together
            float totalWeight = adjustedWeights.Values.Sum();
            float rand = Random.Range(0, totalWeight);

            // Cumulates values until the cumulative reaches the random value then selects that tier
            float cumulative = 0f;
            foreach (var pair in adjustedWeights)
            {
                cumulative += pair.Value;
                if (rand < cumulative) // As soon as the cumulative value is greater than random it selects that tier
                    return pair.Key;
            }

            return AbilityTier.D; // Returns D otherwise
        }
    }
}