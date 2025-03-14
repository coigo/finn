"use client"
import * as React from 'react';
import Link from 'next/link';

import DataSaverOffIcon from '@mui/icons-material/DataSaverOff';
import AttachMoneyIcon from '@mui/icons-material/AttachMoney';
import CompareArrowsIcon from '@mui/icons-material/CompareArrows';

export default function Menu() {


  const itemsTempl = () => {
    return (
      <>
        <Link href={"/dashboard"}>
          <div className='menu-item hover:text-inherit relative md:my-10 md:mx-0 md:hover:bg-amber-500 md:hover:p-2 hover:rounded-2xl '>
            <DataSaverOffIcon fontSize='large' />
          </div>
        </Link>
        <Link href={"/movimentacoes"}>
          <div className='menu-item relative md:my-10 md:mx-0 md:hover:bg-amber-500 md:hover:p-2 hover:rounded-2xl '>
            <CompareArrowsIcon fontSize='large' />
          </div>
        </Link>
        <Link href={"/aportes"}>
          <div className='menu-item relative md:my-10 md:mx-0 md:hover:bg-amber-500 md:hover:p-2 hover:rounded-2xl '>
            <AttachMoneyIcon fontSize='large' />
          </div>
        </Link>
      </>
    )
  }

  return (
    <>
      <div className="menu absolute flex bottom-0 md:left-0 w-full h-fit md:h-full border-gray-600 md:border-0  text-center">
        <div className="flex p-4 md:p-6 md:left-0 w-full h-fit md:h-full md:w-fit">
          <div className='flex md:block justify-around h-fit w-full self-center gap-4 text-gray-500'>

                    {itemsTempl()}
          </div>
        </div>
        <span className="hidden md:block justify-self-end menu-outline"></span>
      </div>
    </>
  );
}
