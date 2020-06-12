using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class KnightsTests
    {
        
        [Test]
        public void PlayerSpawnsInRightPlace()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            Vector3 pos = player.transform.position;
            Vector3 expected_pos = new Vector3(61, 0, (float)80.4);
            Assert.AreEqual(pos, expected_pos);
        }

        [Test]
        public void EnemySpawnsInRightPlace()
        {
            var enemy = GameObject.FindGameObjectWithTag("Enemy");
            var player = GameObject.FindGameObjectWithTag("Player");
            Vector3 player_pos = player.transform.position;
            Vector3 enemy_pos = enemy.transform.position;
            Vector3 expected_pos = player_pos - new Vector3(138.4f, 0, 144.1f);
            
            Assert.IsTrue(Vector3.Distance(enemy_pos, expected_pos) <= 0.5f);
        }
    }
}
