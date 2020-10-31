using UnityEngine;

public class PairMover : BaseMover
{
    private enum PuyoDirection { Top = 0, Right = 1, Bottom = 2, Left = 3 }

    //移動判定
    public override bool CanMove()
    {
        foreach (Puyo p in transform.GetComponentsInChildren<Puyo>())
        {
            if (!p.CanMove()) return false;
        }

        return true;
    }

    public void MoveLeft()
    {
        transform.position += Vector3.left;

        if (!CanMove())
        {
            transform.position -= Vector3.left;
        }
        else if (!CanMoveAboveSpace())
        {
            transform.position -= Vector3.left;
            AdjustToBelow();
            transform.position += Vector3.left;
        }
    }

    public void MoveRight()
    {
        transform.position += Vector3.right;

        if (!CanMove())
        {
            transform.position -= Vector3.right;
        }
        else if (!CanMoveAboveSpace())
        {
            transform.position -= Vector3.right;
            AdjustToBelow();
            transform.position += Vector3.right;
        }
    }

    public void TurnLeft(Vector3 rotatePosition)
    {
        transform.RotateAround(transform.TransformPoint(rotatePosition),
                new Vector3(0, 0, 1), 90);
        
        if (!CanMove())
        {
            var dir = GetDirection();

            if (!KickWall(dir))
            {
                transform.RotateAround(transform.TransformPoint(rotatePosition),
                    new Vector3(0, 0, 1), -90);
            }
        }
    }

    public void TurnRight(Vector3 rotatePosition)
    {
        transform.RotateAround(transform.TransformPoint(rotatePosition),
                new Vector3(0, 0, 1), -90);
        
        if (!CanMove())
        {
            var dir = GetDirection();

            if (!KickWall(dir))
            {
                transform.RotateAround(transform.TransformPoint(rotatePosition),
                    new Vector3(0, 0, 1), 90);
            }
        }
    }

    //一つ上なら移動できるか判定
    private bool CanMoveAboveSpace()
    {
        foreach (Transform c in transform)
        {
            var x = Mathf.RoundToInt(c.transform.position.x);
            var y = Mathf.CeilToInt(c.transform.position.y);

            if (field[x, y] != null) return false;
        }

        return true;
    }

    //下へずらす
    private void AdjustToBelow()
    {
        var p = transform.position;
        int y = Mathf.FloorToInt(p.y);

        transform.position = new Vector3(p.x, y);
    }

    //壁けり
    private bool KickWall(PuyoDirection puyoDir)
    {
        if (puyoDir == PuyoDirection.Left)
        {
            if (!CanMoveTo(Vector3.right)) return false;

            MoveRight();
        }
        else if (puyoDir == PuyoDirection.Right)
        {
            if (!CanMoveTo(Vector3.left)) return false;

            MoveLeft();
        }
        else if (puyoDir == PuyoDirection.Bottom)
        {
            if (!CanMoveTo(Vector3.up)) return false;

            transform.position += Vector3.up;
        }

        return true;
    }

    //移動判定後元の位置に戻す
    private bool CanMoveTo(Vector3 dir)
    {
        bool canMove;

        transform.position += dir;
        canMove = CanMove();
        transform.position -= dir;

        return canMove;
    }

    //子ぷよの方向を取得
    private PuyoDirection GetDirection()
    {
        var a = transform.rotation.eulerAngles.z;

        if (a == 90) return PuyoDirection.Left;

        if (a == 270) return PuyoDirection.Right;

        if (a == 180) return PuyoDirection.Bottom;

        return PuyoDirection.Top;
    }
}
