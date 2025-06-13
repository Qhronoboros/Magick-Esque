public class WaveBuilder
{
    private Wave _wave = new Wave();

    public WaveBuilder SetCollisionEnemyAmount(int amount)
    {
        _wave.collisionEnemyAmount = amount;
        return this;
    }
    
    public WaveBuilder SetAmountPerSpawn(int amountMin, int amountMax)
    {
        _wave.amountMinPerSpawn = amountMin;
        _wave.amountMaxPerSpawn = amountMax;
        return this;
    }
    
    public WaveBuilder SetSpawnTime(float timeMin, float timeMax)
    {
        _wave.spawnTimeMin = timeMin;
        _wave.spawnTimeMax = timeMax;
        return this;
    }

    public Wave Build() => _wave;
}
