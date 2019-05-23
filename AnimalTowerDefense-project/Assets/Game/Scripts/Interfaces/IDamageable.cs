using System.Collections.Generic;

public interface IDamageable
{
    
    void TakeDamage(float damage);
    float GetCurrentHealth();
}