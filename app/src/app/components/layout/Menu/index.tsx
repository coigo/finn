"use client"
import * as React from 'react';

import { FaChartPie } from "react-icons/fa";
import { FaDollarSign } from "react-icons/fa";
import { GoArrowSwitch } from "react-icons/go";



export default function Menu() {


  return (
    <>
      <div className="menu absolute flex bottom-0 md:left-0 w-full md:w-fit h-fit md:h-full border-gray-600 md:border-0  text-center">
        <div className="flex p-4 md:p-6 md:left-0 w-full h-fit md:h-full md:w-fit">
          <div className='flex md:block justify-around h-fit w-full self-center gap-4 text-gray-500'>

            <a href={"/dashboard"}>
              <div className='menu-item md:my-10 md:mx-0 md:hover:bg-[#de983b] md:hover:p-2 hover:rounded-2xl hover:text-amber-950'>
                <FaChartPie fontSize='large' />
              </div>
            </a>
            <a href={"/movimentacoes"}>
              <div className='menu-item relative md:my-10 md:mx-0 md:hover:bg-[#de983b] md:hover:p-2 hover:rounded-2xl hover:text-amber-950'>
                <GoArrowSwitch fontSize='large' />
              </div>
            </a>
            <a href={"/aportes"}>
              <div className='menu-item relative md:my-10 md:mx-0 md:hover:bg-[#de983b] md:hover:p-2 hover:rounded-2xl hover:text-amber-950'>
                <FaDollarSign  />
              </div>
            </a>
          </div>
        </div>
        {/* <span className="hidden md:block justify-self-end menu-outline"
          style={{ backgroundImage: `linear-gradient(180deg, rgba(2,0,36,0) 10%, ${palette.text.primary} 49%, rgba(0,212,255,0) 90%)` }}></span> */}
      </div>
    </>
  );
}
